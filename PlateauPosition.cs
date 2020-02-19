
namespace MarsRovers.ObjectModels
{
    public class PlateauPosition
    {
        public int X { get; set; }

        public int Y { get; set; }

        public CompassDirection FacingDirection { get; set;}

        public override string ToString()
        {
            return $"{X} {Y} {FacingDirection.ToString()}";
        }
    }
}
