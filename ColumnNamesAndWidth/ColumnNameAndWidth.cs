using Newtonsoft.Json;

namespace ColumnNamesAndWidth
{
    public class ColumnNameAndWidth
    {
        public ColumnNameAndWidth(string name, int width)
        {
            Name = name;
            Width = width;

        }

        #region Name
        [JsonProperty("Name")]
        public string Name { get; set; }
        #endregion

        #region Width
        [JsonProperty("Width")]
        public int Width { get; set; }
        #endregion
    }
}
