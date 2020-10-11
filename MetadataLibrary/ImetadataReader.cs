

namespace MetadataLibrary
{
  
    public interface ImetadataReader
    {
        Metadata ReadMetadata(MetadataBrokerTypes broker, string file);
    }
}

