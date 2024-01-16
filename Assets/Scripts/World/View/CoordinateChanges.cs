namespace World.View
{
    public class CoordinateChanges
    {
        public (int, int) OldPosition { get; }
        public (int, int) NewPosition { get; }

        public CoordinateChanges((int, int) oldPosition, (int, int) newPosition)
        {
            OldPosition = oldPosition;
            NewPosition = newPosition;
        }
    }
}