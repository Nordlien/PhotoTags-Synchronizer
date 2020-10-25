using System;
using System.Collections.Generic;

namespace Exiftool
{

    /*No struct vs Struct
            ----
            EXIF:IFD0	XPKeywords	People|Family|Jan;People/Family/Jan;Tag 8;Tag 10;Altoros;PhotoGalleryTag3;PhotoGalleryTag1;PhotoGalleryTag2;closing tag;Ny tag;grey;eat;smile
            EXIF:IFD0	XPKeywords	People|Family|Jan;People/Family/Jan;Tag 8;Tag 10;Altoros;PhotoGalleryTag3;PhotoGalleryTag1;PhotoGalleryTag2;closing tag;Ny tag;grey;eat;smile
            ---
            IPTC	Keywords	People|Family|Jan---People/Family/Jan---Tag 8---Tag 10---Altoros---PhotoGalleryTag3---PhotoGalleryTag1---PhotoGalleryTag2---closing tag---Ny tag---grey---eat---smile
            IPTC	Keywords	[People||Family||Jan,People/Family/Jan,Tag 8,Tag 10,Altoros,PhotoGalleryTag3,PhotoGalleryTag1,PhotoGalleryTag2,closing tag,Ny tag,grey,eat,smile]
            ---
            XMP:XMP-MP	RegionPersonDisplayName	Jan-Terje Nordlien
            XMP:XMP-MP	RegionPersonEmailDigest	BE2062920F2D910D04F60DBF081AA72DE298B49E
            XMP:XMP-MP	RegionPersonLiveIdCID	7668039091782470394
            XMP:XMP-MP	RegionPersonSourceID	WL:7668039091782470394
            XMP:XMP-MP	RegionRectangle	0.317188, 0.230982, 0.105469, 0.186722
            
            XMP:XMP-MP	RegionInfoMP	{Regions=[{PersonDisplayName=Jan-Terje Nordlien,PersonEmailDigest=BE2062920F2D910D04F60DBF081AA72DE298B49E,PersonLiveIdCID=7668039091782470394,PersonSourceID=WL:7668039091782470394,Rectangle=0.317188|, 0.230982|, 0.105469|, 0.186722}]}
            ---            
            XMP:XMP-microsoft	LastKeywordXMP	People|Family|Jan---People/Family/Jan---Tag 8---Tag 10---Altoros---PhotoGalleryTag3---PhotoGalleryTag1---PhotoGalleryTag2---closing tag---Ny tag---grey---eat---smile
            XMP:XMP-microsoft	LastKeywordIPTC	People|Family|Jan---People/Family/Jan---Tag 8---Tag 10---Altoros---PhotoGalleryTag3---PhotoGalleryTag1---PhotoGalleryTag2---closing tag---Ny tag---grey---eat---smile
            
            XMP:XMP-microsoft	LastKeywordXMP	[People||Family||Jan,People/Family/Jan,Tag 8,Tag 10,Altoros,PhotoGalleryTag3,PhotoGalleryTag1,PhotoGalleryTag2,closing tag,Ny tag,grey,eat,smile]
            XMP:XMP-microsoft	LastKeywordIPTC	[People||Family||Jan,People/Family/Jan,Tag 8,Tag 10,Altoros,PhotoGalleryTag3,PhotoGalleryTag1,PhotoGalleryTag2,closing tag,Ny tag,grey,eat,smile]
            
            XMP:XMP-microsoft	LastKeywordXMP	Julie Monsen Nordlien
            XMP:XMP-microsoft	LastKeywordIPTC	Julie Monsen Nordlien

            XMP:XMP-microsoft	LastKeywordXMP	[Julie Monsen Nordlien]
            XMP:XMP-microsoft	LastKeywordIPTC	[Julie Monsen Nordlien]
            ---
            XMP:XMP-dc	Subject	People|Family|Jan---People/Family/Jan---Tag 8---Tag 10---Altoros---PhotoGalleryTag3---PhotoGalleryTag1---PhotoGalleryTag2---closing tag---Ny tag---grey---eat---smile
            XMP:XMP-dc	Subject	[People||Family||Jan,People/Family/Jan,Tag 8,Tag 10,Altoros,PhotoGalleryTag3,PhotoGalleryTag1,PhotoGalleryTag2,closing tag,Ny tag,grey,eat,smile]
            ---
            XMP:XMP-dc	Subject	Julie Monsen Nordlien
            XMP:XMP-dc	Subject	[Julie Monsen Nordlien]
            ---
            XMP:XMP-iptcExt	PersonInImage	Julie Monsen Nordlien
            XMP:XMP-iptcExt	PersonInImage	[Julie Monsen Nordlien]
            ---

            XMP:XMP-MP	RegionInfoMP	{Regions=[{PersonDisplayName=Kristoffer Monsen Nordlien,PersonSourceID=SKYPE:kristoffer.monsen.nordlien,Rectangle=0.366406|, 0.436458|, 0.064063|, 0.085417},{PersonDisplayName=Julie Monsen Nordlien,Rectangle=0.596094|, 0.571875|, 0.039844|, 0.054167},{PersonDisplayName=Lukas Nordlien,Rectangle=0.741406|, 0.723958|, 0.050000|, 0.066667},{PersonDisplayName=_Ikke marker denne,Rectangle=0.813281|, 0.373958|, 0.024219|, 0.031250}]}

            XMP:XMP-MP	RegionRectangle	0.366406, 0.436458, 0.064063, 0.085417---
                            0.596094, 0.571875, 0.039844, 0.054167---
                            0.741406, 0.723958, 0.050000, 0.066667---
                            0.813281, 0.373958, 0.024219, 0.031250
            XMP:XMP-MP	RegionPersonDisplayName	Kristoffer Monsen Nordlien---Julie Monsen Nordlien---Lukas Nordlien---_Ikke marker denne
            XMP:XMP-MP	RegionPersonSourceID	SKYPE:kristoffer.monsen.nordlien

            ---
            XMP:XMP-mwg-rs	RegionInfo	{AppliedToDimensions={H=2736,Unit=pixel,W=3648},RegionList=[{Area={H=0.08542,W=0.06406,X=0.39844,Y=0.47917},Rotation=0.00000,Type=Face},{Area={H=0.05417,W=0.03984,X=0.61602,Y=0.59896},Name=Julie Monsen Nordlien,Rotation=0.00000,Type=Face},{Area={H=0.06667,W=0.05000,X=0.76641,Y=0.75729},Name=Lukas Nordlien,Rotation=0.00000,Type=Face},{Area={H=0.03125,W=0.02422,X=0.82539,Y=0.38958},Rotation=0.00000,Type=Face}]}
            
            XMP:XMP-mwg-rs	RegionAppliedToDimensionsW	3648
            XMP:XMP-mwg-rs	RegionAppliedToDimensionsH	2736
            XMP:XMP-mwg-rs	RegionAppliedToDimensionsUnit	pixel
            XMP:XMP-mwg-rs	RegionRotation	0.00000---0.00000---0.00000---0.00000
            XMP:XMP-mwg-rs	RegionType	Face---Face---Face---Face
            XMP:XMP-mwg-rs	RegionAreaH	0.08542---0.05417---0.06667---0.03125
            XMP:XMP-mwg-rs	RegionAreaW	0.06406---0.03984---0.05000---0.02422
            XMP:XMP-mwg-rs	RegionAreaX	0.39844---0.61602---0.76641---0.82539
            XMP:XMP-mwg-rs	RegionAreaY	0.47917---0.59896---0.75729---0.38958
            XMP:XMP-mwg-rs	RegionName	Julie Monsen Nordlien---Lukas Nordlien

            XMP:XMP-lr	HierarchicalSubject	Julie Monsen Nordlien---Lukas Nordlien
            XMP:XMP-lr	HierarchicalSubject	[Julie Monsen Nordlien,Lukas Nordlien]

            
             */
    public enum StructTypes
    {
        FieldName,
        Value,
        OpeningCurlyBracket,
        ClosingCurlyBracket,
        OpeningSquareBrackets,
        ClosingSquareBrackets, 
        EOF
    }

    public class StructObject
    {
        public int Level { get; set; } = -1;
        public string Value { get; set; } = null;
        public StructTypes Type { get; set; } = StructTypes.Value;
        public bool IsList { get; set; } = false;
    }

    public class StructDeSerialization
    {
        string structString;
        int readLocation = 0;
        int level = -1;
        bool isInsideList = false;

        public StructDeSerialization(string structString)
        {
            this.structString = structString;
        }

        public int Level { get => level; set => level = value; }

        /*
        Serialization
        1. Escape the following characters in string values (structure field values and list items) by adding a leading pipe symbol (|):
            - pipe symbols (|) and commas (,) anywhere in the string (Note: Any other character may be escaped by adding a leading pipe symbol without effect.)
            - closing curly brackets (}) anywhere in structure field values
            - closing square brackets (]) anywhere in list items
            - an opening curly ({) or square ([) bracket, or whitespace character (SPACE, TAB, CR or LF) if it appears at the beginning of 
              the string
             
        2. Enclose structures in curly brackets. Use an equal sign (=) to separate field names from their corresponding values, 
           and a comma between structure fields.
        3. Enclose lists in square brackets, with a comma between list items.
        4. Optional whitespace padding may be added anywhere except inside a structure field name, or inside or after a string value, 
           and an optional comma may be added after the last field in a structure.
        */
        //{Regions=[{PersonDisplayName=Kristoffer Monsen Nordlien,PersonSourceID=SKYPE:kristoffer.monsen.nordlien,Rectangle=0.366406|, 0.436458|, 0.064063|, 0.085417},{PersonDisplayName=Julie Monsen Nordlien,Rectangle=0.596094|, 0.571875|, 0.039844|, 0.054167},{PersonDisplayName=Lukas Nordlien,Rectangle=0.741406|, 0.723958|, 0.050000|, 0.066667},{PersonDisplayName=_Ikke marker denne,Rectangle=0.813281|, 0.373958|, 0.024219|, 0.031250}]}
        //{Area={H=0.139205,Unit=normalized,W=0.104403,X=0.232955,Y=0.202415},Name=Julie Monsen Nordlien,Type=Face},
        public bool Read(out StructObject structObject)
        {
            structObject = new StructObject();

            bool escape = false;
            bool addCharToValue;
            bool continueRead = true;

            if (readLocation == structString.Length)
            {
                structObject.Type = StructTypes.EOF;
                return false; //Nothing more to read
            }

            while (readLocation < structString.Length && continueRead)
            {
                char readChar = structString[readLocation];
                addCharToValue = true;

                if (!escape)
                {
                    switch (readChar)
                    {
                        case '|':
                            escape = true;
                            addCharToValue = false;
                            continueRead = true;
                            break;
                        case ',':
                            //isInsideList = true;
                            //structObject.Type = StructTypes.FieldName;
                            addCharToValue = false;
                            continueRead = false;
                            break;

                        case '=':
                            //isInsideList = true;
                            structObject.Type = StructTypes.FieldName;
                            addCharToValue = false;
                            continueRead = false;
                            break;

                        case '[':
                            isInsideList = true;
                            structObject.Type = StructTypes.OpeningSquareBrackets;
                            addCharToValue = false;
                            continueRead = false;
                            break;                        
                        case ']':
                            isInsideList = false;
                            structObject.Type = StructTypes.ClosingSquareBrackets;
                            addCharToValue = false;
                            continueRead = false;
                            break;

                        case '{':
                            Level++;
                            //isInsideList = false;
                            structObject.Type = StructTypes.OpeningCurlyBracket;
                            addCharToValue = false;
                            continueRead = false;
                            break;
                        case '}':
                            Level--;
                            //isInsideList = false;
                            structObject.Type = StructTypes.ClosingCurlyBracket;
                            addCharToValue = false;
                            continueRead = false;
                            break;
                        default:
                            break;
                    }
                } else
                {
                    escape = false;
                    addCharToValue = true;
                }

                if (addCharToValue) structObject.Value += readChar;
                
                readLocation++;
            }

            structObject.Level = Level;
            structObject.IsList = isInsideList;
            if (structObject.Type == StructTypes.FieldName) structObject.Value = structObject.Value.TrimStart();

            return readLocation <= structString.Length; //New also read last sign
        }

        public static List<string> GetListOfValues(string parameter)
        {
            List<string> values = new List<string>();

            StructDeSerialization structDeSerialization = new StructDeSerialization(parameter);
            StructObject structObject;
            while (structDeSerialization.Read(out structObject))
            {
                switch (structObject.Type) 
                {
                    case StructTypes.EOF:
                    case StructTypes.OpeningSquareBrackets:
                    case StructTypes.OpeningCurlyBracket:
                        if (structObject.Value != null) values.Add(structObject.Value);
                        break;
                    case StructTypes.FieldName:
                        //Should not occure
                        break;
                    case StructTypes.ClosingCurlyBracket:
                    case StructTypes.ClosingSquareBrackets:
                    case StructTypes.Value:
                    default:
                        //if (structObject.Type == StructTypes.Value) 
                        if (structObject.Value != null) values.Add(structObject.Value);
                        break;
                }
            }

            return values;
        }

    }
}