using System.Collections.Generic;
using System.Linq;
using Characters;
using PathFinder.View;
using UnityEngine;
using World;

namespace PathFinder
{
    public class PathFinding
    {
        private List<Node> openSet;
        private HashSet<Node> closedSet;

        public List<(int, int)> Find((int, int) startPos, (int, int) targetPos, MapController mapController, CharactersController charactersController)
        {
            if (charactersController.IsCharacterOccupyingPosition(targetPos))
            {
                return new List<(int, int)>();
            }

            Node[][] nodeMap = CreateNodeMap(mapController.MapHeight(), mapController.MapWidth());
            
            openSet = new List<Node>();
            closedSet = new HashSet<Node>();
            
            Node targetNode = nodeMap[targetPos.Item1][targetPos.Item2];
            Node startNode = nodeMap[startPos.Item1][startPos.Item2];

            openSet.Add(startNode);
            while (openSet.Count > 0)
            {
                Node currentNode = FindOpenSetNodeWithLowestFCost();
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);
                
                if (currentNode == targetNode)
                {
                    return RetracePath(startNode, targetNode);
                }

                GetNeighbours(currentNode, mapController.MapWidth(), nodeMap)
                    .Where(neighbour => !closedSet.Contains(neighbour))
                    .Where(neighbour => mapController.GetNonVoidTilePositions().Contains((neighbour.xAxis, neighbour.zAxis)))
                    .ToList()
                    .ForEach(neighbour => { UpdateNeighbourNode(currentNode, targetNode, neighbour); });
            }

            return new List<(int, int)>();
        }
        
        private Node[][] CreateNodeMap(int mapHeight, int mapWidth)
        {
            Node[][] nodeMap = new Node[mapHeight][];
            for (int i = 0; i < mapHeight; i++)
            {
                nodeMap[i] = new Node[mapWidth];
                for (int j = 0; j < mapWidth; j++)
                {
                    nodeMap[i][j] = new Node();
                    nodeMap[i][j].xAxis = i;
                    nodeMap[i][j].zAxis = j;
                }
            }

            return nodeMap;
        }

        private void UpdateNeighbourNode(Node currentNode, Node targetNode, Node neighbour)
        {
            float movementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
            if (movementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
            {
                neighbour.gCost = (int) movementCostToNeighbour;
                neighbour.hCost = GetDistance(neighbour, targetNode);
                neighbour.AdjacentNode = currentNode;

                if (!openSet.Contains(neighbour))
                {
                    openSet.Add(neighbour);
                }
            }
        }

        private List<(int, int)> RetracePath(Node startNode, Node targetNode)
        {
            List<(int, int)> newPath = new List<(int, int)>();
            Node currentNode = targetNode;

            while (currentNode != startNode)
            {
                newPath.Add((currentNode.xAxis, currentNode.zAxis));
                currentNode = currentNode.AdjacentNode;
            }
            newPath.Reverse();

            return newPath;
        }

        private List<Node> GetNeighbours(Node currentNode, int mapSize, Node[][] map)
        {
            List<Node> neighbours = new List<Node>();

            if (currentNode.xAxis - 1 >= 0)
                neighbours.Add(map[currentNode.xAxis - 1][currentNode.zAxis]);
            if (currentNode.xAxis < mapSize - 1)
                neighbours.Add(map[currentNode.xAxis + 1][currentNode.zAxis]);
            if (currentNode.zAxis - 1 >= 0 )
                neighbours.Add(map[currentNode.xAxis][currentNode.zAxis - 1]);
            if (currentNode.zAxis < mapSize - 1)
                neighbours.Add(map[currentNode.xAxis][currentNode.zAxis + 1]);
            return neighbours;
        }

        private int GetDistance(Node nodeA, Node nodeB)
        {
            int distX = Mathf.Abs(nodeA.xAxis - nodeB.xAxis);
            int distZ = Mathf.Abs(nodeA.zAxis - nodeB.zAxis);

            return (int) Mathf.Sqrt(Mathf.Pow(distX, 2) + Mathf.Pow(distZ, 2));
        }

        private Node FindOpenSetNodeWithLowestFCost()
        {
            Node currentNode = openSet.First();
            openSet.Where(node => node != currentNode).ToList().ForEach(node => 
            {
                if (node.fCost <= currentNode.fCost && node.hCost < currentNode.hCost)
                    currentNode = node;
            });

            return currentNode;
        }
    }
}
