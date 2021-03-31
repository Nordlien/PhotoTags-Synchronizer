namespace LocationNames
{
    public class LocationCoordinateAndDescription
    {
        private LocationCoordinate locationCoordinate = null;
        private LocationDescription locationDescription = null;

        public LocationCoordinateAndDescription()
        {
            locationCoordinate = new LocationCoordinate();
            locationDescription = new LocationDescription();
        }

        public LocationCoordinateAndDescription(LocationCoordinate coordinate, LocationDescription description)
        {
            Coordinate = coordinate;
            Description = description;            
        }

        public LocationDescription Description { get => locationDescription; set => locationDescription = value; }
        public LocationCoordinate Coordinate { get => locationCoordinate; set => locationCoordinate = value; }
    }
}

