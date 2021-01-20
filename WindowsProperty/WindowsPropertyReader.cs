using DataGridViewGeneric;
using MetadataLibrary;
using Microsoft.WindowsAPICodePack.Shell;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WinProps;
using PropertyKey = WinProps.PropertyKey;

namespace WindowsProperty
{
    
    public class WindowsPropertyReader
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public void Write(DataGridView dataGridView, int columnIndex)
        {
            List<PropertyKey> notSet = new List<PropertyKey>();

            DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
            if (dataGridViewGenericColumn == null) return; //continue;

            string fullFileName = dataGridViewGenericColumn.FileEntryAttribute.FileFullPath;
    
            using (PropertyStore propertyStore = new PropertyStore(fullFileName, PropertyStore.GetFlags.ReadWrite | PropertyStore.GetFlags.SlowItem))
            {

                for (int rowIndex = 0; rowIndex < DataGridViewHandler.GetRowCountWithoutEditRow(dataGridView); rowIndex++)
                {
                    bool isReadOnly = DataGridViewHandler.GetCellReadOnly(dataGridView, columnIndex, rowIndex);
                    //object tag = DataGridViewHandler.GetCellStatus(dataGridView, columnIndex, rowIndex);
                    //DataGridViewGenericCellStatus dataGridViewGenericCellStatus = DataGridViewHandler.GetCellStatus(dataGridView, columnIndex, rowIndex);
                    DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                    object value = DataGridViewHandler.GetCellValue(dataGridView, columnIndex, rowIndex);

                    //if (dataGridViewGenericCellStatus.tag is PropertyKey && value is string)
                    if (value is string)
                    {
                        PropertyKey propertyKey = dataGridViewGenericRow.PropertyKey;
                        string valueString = (string)value;

                        if (!isReadOnly && propertyStore.IsEditable(propertyKey))
                        {
                            try
                            {
                                //if (!(bool)_displayProps[key].Tag) //Check if read only
                                {
                                    using (PropertyDescription propertyDescription = new PropertyDescription(propertyKey))
                                    {
                                        if (!string.IsNullOrEmpty(valueString))
                                        {
                                            if (propertyDescription.TypeFlags.IsMultiValued)
                                            {
                                                valueString = valueString.Replace("\r\n", ";");
                                                PropVariant val = PropVariant.FromStringAsVector(valueString);
                                                propertyStore.SetValue(propertyKey, val);
                                            }
                                            else
                                            {
                                                switch (propertyDescription.PropertyType)
                                                {
                                                    case VarEnum.VT_UI4:
                                                        string cleanedString = new string(valueString.Where(char.IsDigit).ToArray());
                                                        UInt32 valueUint32;
                                                        if (UInt32.TryParse(cleanedString, out valueUint32))
                                                        {
                                                            if (propertyKey.CanonicalName == "System.Rating")
                                                                valueUint32 = (UInt32)Metadata.ConvertRatingStarsToRatingPercent((byte)valueUint32);

                                                            propertyStore.SetValue(propertyKey, new PropVariant(valueUint32, propertyDescription.PropertyType));
                                                        }
                                                        else propertyStore.SetValue(propertyKey, new PropVariant());
                                                        break;
                                                    case VarEnum.VT_DATE:
                                                    case VarEnum.VT_FILETIME:
                                                        string cleanedString2 = new string(valueString.Where(c => !char.IsControl(c)).ToArray());
                                                        // https://social.msdn.microsoft.com/Forums/windows/en-US/61687b42-9882-4025-8b3a-b3251f121f94/how-to-prevent-a-textbox-to-accept-unicode-control-characters?forum=winforms
                                                        cleanedString2 = Regex.Replace(cleanedString2, @"[\u200e-\u200f]", string.Empty);
                                                        cleanedString2 = Regex.Replace(cleanedString2, @"[\u202a-\u202e]", string.Empty);
                                                        cleanedString2 = Regex.Replace(cleanedString2, @"[\u206a-\u206f]", string.Empty);
                                                        cleanedString2 = Regex.Replace(cleanedString2, @"[\u001e-\u001f]", string.Empty);

                                                        DateTime valueDateTime;
                                                        if (DateTime.TryParse(cleanedString2, out valueDateTime))
                                                        {
                                                            propertyStore.SetValue(propertyKey, new PropVariant(valueDateTime.ToUniversalTime(), propertyDescription.PropertyType));
                                                        }
                                                        break;
                                                    default:
                                                        propertyStore.SetValue(propertyKey, new PropVariant(valueString, propertyDescription.PropertyType));
                                                        break;
                                                }
                                            }
                                            //store.SetValue(key, new PropVariant(_displayProps[key], desc.PropertyType));
                                        }
                                        else propertyStore.SetValue(propertyKey, new PropVariant());
                                    }
                                }
                                continue;
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex);
                            }
                            notSet.Add(propertyKey);
                        }
                    }
                }
                propertyStore.Commit();
            }
            
            if (notSet.Count > 0)
            {
                throw new Exception ("The following keys were not set:\n\t" + string.Join("\n\t", notSet.Select(k => k.CanonicalName)));
            }

        }


        public List<DataGridViewGenericRowAndValue> Read(string fullFileName)
        {
            List<DataGridViewGenericRowAndValue> dataGridViewGenericRowsAndValueList = new List<DataGridViewGenericRowAndValue>();
            string fileExtension = Path.GetExtension(fullFileName);
            Dictionary<PropertyKey, PropVariant> filePropertiesAndVariant = new Dictionary<WinProps.PropertyKey, PropVariant>();
            Dictionary<PropertyKey, bool> filePropertiesReadOnly = new Dictionary<WinProps.PropertyKey, bool>();

            try
            {
                using (PropertyStore propertyStoreExtension = new PropertyStore())
                {
                    propertyStoreExtension.AddDefaultsByExtension(fileExtension);

                    foreach (PropertyKey propertyKey in propertyStoreExtension)
                    {
                        try
                        {
                            using (PropertyDescription propertyDescription = new PropertyDescription(propertyKey))
                            {

                                PropVariant propVariant = propertyStoreExtension.GetValue(propertyKey);                                
                                filePropertiesAndVariant[propertyKey] = propVariant;
                                //filePropertiesReadOnly[propertyKey] = propertyStoreExtension.IsEditable(propertyKey);
                            }
                        }
                        catch { }
                    }
                }
            }
            catch { }
            
            try
            {
                using (PropertyStore propertyStoreFile = new PropertyStore(fullFileName, PropertyStore.GetFlags.Default))
                {
                    foreach (PropertyKey propertyKey in propertyStoreFile)
                    {
                        try
                        {
                            using (PropertyDescription propertyDescription = new PropertyDescription(propertyKey))
                            {
                                PropVariant propVariant = propertyStoreFile.GetValue(propertyKey);                                
                                filePropertiesAndVariant[propertyKey] = propVariant;
                                filePropertiesReadOnly[propertyKey] = !propertyStoreFile.IsEditable(propertyKey);
                            }
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(this, "Unable to load properties for " + Path.GetFileName(openFileDialog1.FileName) + "\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return;
            }

            PropertyDescriptionList propertyDescriptionList = null;
            try
            {
                propertyDescriptionList = new PropertyDescriptionList(fileExtension, PropertyKey.Keys["System.PropList.FullDetails"]);
            }
            catch
            {
            }

            if (propertyDescriptionList != null)
            {
                string groupName = "Unknown";
                bool isReadOnlyGroup = false;
                for (int i = 0; i < propertyDescriptionList.Count; ++i)
                {
                    using (PropertyDescription propertyDescription = propertyDescriptionList[i])
                    {
                        using (PropertyKey key = propertyDescription.PropertyKey)
                        {
                            if (propertyDescription.TypeFlags.IsGroup)
                            {
                                isReadOnlyGroup = true;
                                if (key.Equals(PropertyKey.Keys["System.PropGroup.Origin"])) isReadOnlyGroup = false;
                                if (key.Equals(PropertyKey.Keys["System.PropGroup.Description"])) isReadOnlyGroup = false;
                                //isReadOnlyGroup = key.Equals(PropertyKey.Keys["System.PropGroup.FileSystem"]);

                                groupName = propertyDescription.DisplayName;
                                dataGridViewGenericRowsAndValueList.Add(new DataGridViewGenericRowAndValue(propertyDescription.DisplayName, key, isReadOnlyGroup));
                            }
                            else
                            {
                                bool readOnly = isReadOnlyGroup;
                               

                                bool isIsMultiValued = propertyDescription.TypeFlags.IsMultiValued;
                                
                                string textValue = "";
                                if (filePropertiesAndVariant.ContainsKey(key)) // && !isFileGroup)
                                {
                                    if (propertyDescription.TypeFlags.IsMultiValued)
                                    {
                                        if (filePropertiesAndVariant[key].IsVector)
                                            textValue = string.Join(Environment.NewLine, filePropertiesAndVariant[key].ToType<string>().Split(new[] { "; ", ";" }, StringSplitOptions.RemoveEmptyEntries));
                                        else
                                            textValue = propertyDescription.FormatForDisplay(filePropertiesAndVariant[key], PropertyDescription.FormatFlags.Default);
                                    }
                                    else
                                        textValue = propertyDescription.FormatForDisplay(filePropertiesAndVariant[key], PropertyDescription.FormatFlags.Default);

                                    string test = "";
                                    if (!filePropertiesReadOnly.ContainsKey(key) || filePropertiesReadOnly[key])
                                    {
                                        readOnly = true;
                                        test = "*";
                                    }
                                    else
                                    {
                                        //		propertyDescription.CanonicalName	"System.Image.ImageID"	string

                                        /*File/Properties	C:\Users\nordl\OneDrive\Pictures JTNs OneDrive\TestTags\IMG_1267.jpg
                                        Metering mode	Pattern
                                        Flash mode	No flash, compulsory */
                                    }
                                    if (key.Equals(PropertyKey.Keys["System.Image.ImageID"])) readOnly = true;
                                    if (key.Equals(PropertyKey.Keys["System.Photo.ISOSpeed"])) readOnly = true;
                                    if (key.Equals(PropertyKey.Keys["System.Photo.MeteringMode"])) readOnly = true;
                                    if (key.Equals(PropertyKey.Keys["System.Photo.Flash"])) readOnly = true;

                                    dataGridViewGenericRowsAndValueList.Add(
                                        new DataGridViewGenericRowAndValue(
                                        groupName, 
                                        propertyDescription.DisplayName + test, key, 
                                        readOnly ? ReadWriteAccess.ForceCellToReadOnly : ReadWriteAccess.DefaultReadOnly, 
                                        isIsMultiValued, readOnly, textValue));
                                }

                            }
                        }
                    }
                }
            }

            return dataGridViewGenericRowsAndValueList;
        }


        public Image GetThumbnail (string fullFileName)
        {
            Bitmap image = null;
            //Console.WriteLine("WindowsPropertyReader.GetThumbnail:" + File.Exists(fullFileName));
            if (File.Exists(fullFileName))
            {
                ShellFile shellFile = ShellFile.FromFilePath(fullFileName);
                try
                {
                    shellFile.Thumbnail.FormatOption = ShellThumbnailFormatOption.ThumbnailOnly;
                    image = shellFile.Thumbnail.ExtraLargeBitmap;

                }
                catch (Exception ex)
                {
                    Logger.Trace("Shell Thumbnail failed " + fullFileName + " " + ex.Message);
                }

                try
                {
                    if (image == null) image = shellFile.Thumbnail.LargeBitmap;
                }
                catch (Exception ex)
                {
                    Logger.Trace("Shell Thumbnail failed " + fullFileName + " " + ex.Message);
                }

                try
                {
                    if (image == null) image = shellFile.Thumbnail.MediumBitmap;
                }
                catch (Exception ex)
                {
                    Logger.Trace("Shell Thumbnail failed " + fullFileName + " " + ex.Message);
                }

                try
                {
                    if (image == null) image = shellFile.Thumbnail.SmallBitmap;
                }
                catch (Exception ex)
                {
                    Logger.Trace("Shell Thumbnail failed " + fullFileName + " " + ex.Message);
                }
            } else
            {
                Logger.Error("File doesn't exist anymore. " + fullFileName);
            }

            return image;
        }
    }

}
