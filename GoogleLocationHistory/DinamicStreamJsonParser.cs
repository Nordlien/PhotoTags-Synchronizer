using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GoogleLocationHistory
{
    public class DinamicStreamJsonParser
    {
        public delegate void ObjectFound(object obj, long readPosition, long fileLength);
        public delegate void ObjectFoundWithName(object obj, string name);

        public StreamReader StreamReader;
        public Dictionary<string, Type> PropertyToType;
        public ObjectFound ObjectFoundCallback;
        public ObjectFoundWithName NewTypeFoundCallback = null;

        public void Parse()
        {
            JsonTextReader reader = new JsonTextReader(StreamReader);
            reader.SupportMultipleContent = true;

            var serializer = new JsonSerializer();

            string currentFound = "";
            string oldFound = "";
            int startObject = 0;

            while (reader.Read())
            {
                
                if (reader.TokenType == JsonToken.StartObject)
                {
                    startObject++;
                }
                if (reader.TokenType == JsonToken.EndObject)
                {
                    startObject--;
                }
                if (startObject == 1)
                {
                    if (reader.TokenType == JsonToken.PropertyName &&
                        PropertyToType.ContainsKey((string)reader.Value)) currentFound = (string)reader.Value;
                }

                if (startObject == 2 && reader.TokenType == JsonToken.StartObject)
                {

                    if (PropertyToType.ContainsKey(currentFound))
                    {

                        var type = PropertyToType[currentFound];

                        var o = typeof(DinamicStreamJsonParser)
                            .GetMethod("Deserialize")
                            .MakeGenericMethod(type)
                            .Invoke(this, new object[] { reader, serializer });

                        if (NewTypeFoundCallback != null && oldFound != currentFound)
                            NewTypeFoundCallback(o, currentFound);

                        ObjectFoundCallback(o, StreamReader.BaseStream.Position, StreamReader.BaseStream.Length);

                        oldFound = currentFound;

                    }

                    startObject--;
                }

            }

        }

        public T Deserialize<T>(JsonTextReader reader, JsonSerializer serializer)
        {
            return serializer.Deserialize<T>(reader);
        }

    }

    
}

