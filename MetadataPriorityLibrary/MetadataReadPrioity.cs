using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace MetadataPriorityLibrary
{
    public class MetadataReadPrioity
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        #region MetadataGroupPrioity
        public Dictionary<MetadataPriorityKey, MetadataPriorityValues> MetadataPrioityDictionary { get; set; } = null;
        private Dictionary<string, int> metadataCompositeTagHighestPrioity = new Dictionary<string, int>();
        private bool isMetadataGroupPrioityDictionaryDirty = false;

        public int GetCompositeTagsHighestPrioity(string compositeTag)
        {
            if (!metadataCompositeTagHighestPrioity.ContainsKey(compositeTag))
            {
                metadataCompositeTagHighestPrioity.Add(compositeTag, Int32.MinValue); 
            }
            return metadataCompositeTagHighestPrioity[compositeTag];
        }

        public string CreteFilename()
        {
            //string jsonPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PhotoTagsSynchronizer");
            string jsonPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            return Path.Combine(jsonPath, "MetadataPrioities.json");
        }

        public void ReadOnlyOnce()
        {
            if (MetadataPrioityDictionary == null) 
            {
                MetadataPrioityDictionary = new Dictionary<MetadataPriorityKey, MetadataPriorityValues>();
                Read();
            }
        }
        public void Read()
        {
            string filename = CreteFilename();
            if (File.Exists(filename))
            {
                List<MetadataPriorityGroup> metadataGroupPriorityList = null;
                try
                {
                    metadataGroupPriorityList = JsonConvert.DeserializeObject<List<MetadataPriorityGroup>>(File.ReadAllText(filename));
                }
                catch (Exception ex)
                {
                    Logger.Error("Can't read " + filename + " file" + ex.Message);
                }
                if (metadataGroupPriorityList == null) metadataGroupPriorityList = new List<MetadataPriorityGroup>();
                foreach (MetadataPriorityGroup metadataGroupPriority in metadataGroupPriorityList)
                {
                    if (!MetadataPrioityDictionary.ContainsKey(metadataGroupPriority.MetadataPriorityKey))
                        MetadataPrioityDictionary.Add(metadataGroupPriority.MetadataPriorityKey, metadataGroupPriority.MetadataPriorityValues);
                }
            }
        }

        public void Write()
        {
            if (!isMetadataGroupPrioityDictionaryDirty) return;

            List<MetadataPriorityGroup> metadataGroupPriorityList = new List<MetadataPriorityGroup>();
            foreach (KeyValuePair<MetadataPriorityKey, MetadataPriorityValues> entry in MetadataPrioityDictionary)
            {
                metadataGroupPriorityList.Add(new MetadataPriorityGroup(entry.Key, entry.Value));
            }
            string filename = CreteFilename();
            File.WriteAllText(filename, JsonConvert.SerializeObject(metadataGroupPriorityList, Newtonsoft.Json.Formatting.Indented));
            isMetadataGroupPrioityDictionaryDirty = false;
        }

        public void Add(string region, string tag, string composite)
        {
            Add(new MetadataPriorityKey(region, tag), new MetadataPriorityValues(composite, 100));
        }
        public void MetadataGroupPrioityAdd(MetadataPriorityKey metadataPriorityKey, string composite)
        {
            Add(metadataPriorityKey, new MetadataPriorityValues(composite, 100));
        }

        public void Add(MetadataPriorityKey metadataPriorityKey, MetadataPriorityValues metadataPriorityValues)
        {
            if (!MetadataPrioityDictionary.ContainsKey(metadataPriorityKey))
            {
                isMetadataGroupPrioityDictionaryDirty = true;
                MetadataPrioityDictionary.Add(metadataPriorityKey, metadataPriorityValues);
            }
        }

        public int Get(string region, string tag, string compositeTag)
        {
            return Get(new MetadataPriorityKey(region, tag), compositeTag).Priority;
        }

        public MetadataPriorityValues Get(MetadataPriorityKey metadataPriorityKey, string compositeTag)
        {
            if (!MetadataPrioityDictionary.ContainsKey(metadataPriorityKey))
            {
                MetadataGroupPrioityAdd(metadataPriorityKey, compositeTag);
            }
            return MetadataPrioityDictionary[metadataPriorityKey];
        }
        #endregion
    }
}