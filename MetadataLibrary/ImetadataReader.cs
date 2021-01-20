

namespace MetadataLibrary
{
  
    public interface ImetadataReader
    {
        Metadata Read(MetadataBrokerType broker, string file);
    }
}

