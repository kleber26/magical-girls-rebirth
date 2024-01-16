namespace PathFinder.View
{
    public class Node
    {
        public Node AdjacentNode;
        public int xAxis;
        public int zAxis;

        // Distance between: Player -> Neighbour
        public int gCost;
        // Distance between: Neighbour -> Target
        public int hCost;
        // Distance between: Player -> Target
        public int fCost
        {
            get { return gCost + hCost; }
        }

        public Node()
        {
            gCost = 0;
        }
    }
}