

namespace MetadataLibrary
{
  
    public interface ImetadataReader
    {
        Metadata Read(MetadataBrokerTypes broker, string file);
    }
}

