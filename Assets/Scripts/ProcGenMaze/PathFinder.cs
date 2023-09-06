using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    private CellType[,] grid;
    private PathNode[,] pathNodes;

    private List<PathNode> openList;
    private List<PathNode> closeList;

    private int width;
    private int height;

    public CellType[,] getGrid() 
    {
        return grid;
    }

    public List<PathNode> findPath(Vector3 origin, Vector3 target)
    {
        grid[(int)target.x, (int)target.z] = CellType.Aux;
        PathNode startNode = pathNodes[(int)origin.x, (int)origin.z];
        PathNode endNode = pathNodes[(int)target.x, (int)target.z];

        //Debug.Log("Getting from " + origin + " to " + target);

        openList = new List<PathNode> { startNode };
        closeList = new List<PathNode>();

        startNode.setGCost(0);
        startNode.setHCost(CalculateDistanceCost(startNode, endNode));
        startNode.calculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            //Debug.Log("currentNode " + currentNode.getLocation() + " gCost " + currentNode.getGCost() + " hCost " + currentNode.getHCost());

            //if(CalculateDistanceCost(currentNode, endNode) <= 2)
            if (currentNode.getLocation() == endNode.getLocation())
            {
                grid[(int)target.x, (int)target.z] = CellType.Room;
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            foreach( PathNode neighbour in GetNeighbourList(currentNode)) 
            {
                if (!closeList.Contains(neighbour)) 
                {
                    int tentativeGCost = currentNode.getGCost() + CalculateDistanceCost(currentNode, neighbour);
                    if (tentativeGCost < neighbour.getGCost()) 
                    {
                        neighbour.setCameFrom(currentNode);
                        neighbour.setGCost(tentativeGCost);
                        neighbour.setHCost(CalculateDistanceCost(neighbour, endNode));
                        neighbour.calculateFCost();

                        if (!openList.Contains(neighbour)) 
                        {
                            openList.Add(neighbour);
                        }
                    }
                }
            }
        }

        return null;

    }

    private void UpdateGrid(List<PathNode> path) 
    {
        for (int i = 0; i < path.Count; i++) 
        {
            PathNode node = path[i];
            //Debug.Log(node.getLocation());
            CellType current = grid[(int)node.getLocation().x, (int)node.getLocation().z];
            if (current != CellType.Room) 
            {
                grid[(int)node.getLocation().x, (int)node.getLocation().z] = CellType.Hallway;
            }
        }
    }

    private List<PathNode> CalculatePath(PathNode endNode) 
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;

        while (currentNode.getCameFrom() != null) 
        {
            //Debug.Log(currentNode.getLocation());
            path.Add(currentNode.getCameFrom());
            currentNode = currentNode.getCameFrom();
        }

        path.Reverse();
        path.RemoveAt(0);

        UpdateGrid(path);
        return path;
    }

    private CellType getCellType(PathNode node) 
    {
        return grid[(int)node.getLocation().x, (int)node.getLocation().z];
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode) 
    {
        int x = (int) currentNode.getLocation().x;
        int z = (int) currentNode.getLocation().z;

        List<PathNode> neighbourList = new List<PathNode>();

        //Left
        if (x - 1 >= 0 && getCellType(pathNodes[x - 1, z]) != CellType.Room) 
        {
            neighbourList.Add(pathNodes[x - 1, z]);
        }
        //Right
        if(x + 1 < width && getCellType(pathNodes[x + 1, z]) != CellType.Room) 
        {
            neighbourList.Add(pathNodes[x + 1, z]);
        }
        //Down
        if (z - 1 >= 0 && getCellType(pathNodes[x, z - 1]) != CellType.Room) 
        {
            neighbourList.Add(pathNodes[x, z - 1]);
        }
        //Up
        if (z + 1 < height && getCellType(pathNodes[x, z + 1]) != CellType.Room) 
        {
            neighbourList.Add(pathNodes[x, z + 1]);
        }

        return neighbourList;
    }


    private int CalculateDistanceCost(PathNode a, PathNode b) 
    {
        //Manhattan distance
        int dist = (int) (Mathf.Abs( a.getLocation().x - b.getLocation().x ) + Mathf.Abs( a.getLocation().z - b.getLocation().z ));

        return dist;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodes) 
    {
        PathNode lowestNode = pathNodes[0];
        for(int i = 1; i < pathNodes.Count; i++) 
        {
            if (pathNodes[i].getFCost() < lowestNode.getFCost()) 
            {
                lowestNode = pathNodes[i];
            }
        }

        return lowestNode;
    }

    public PathFinder(CellType[,] g, int width, int height) 
    {
        grid = g;
        this.width = width;
        this.height = height;
        pathNodes = new PathNode[width, height];

        for (int i = 0; i < grid.GetLength(0); i++) 
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                PathNode node = new PathNode(new Vector3(i, 0, j));
                node.setGCost(int.MaxValue);
                node.calculateFCost();
                node.setCameFrom(null);
                pathNodes[i, j] = node;
            }
        }
    }
}


public class PathNode
{
    private Vector3 location;

    private int gCost;
    private int hCost;
    private int fCost;

    private PathNode cameFrom;


    public void setGCost(int cost)
    {
        gCost = cost;
    }

    public void setHCost(int cost)
    {
        hCost = cost;
    }

    public void setCameFrom(PathNode node) 
    {
        cameFrom = node;
    }

    public PathNode getCameFrom() 
    {
        return cameFrom;
    }

    public int getGCost() 
    {
        return gCost;
    }

    public int getHCost() 
    {
        return hCost;
    }

    public int getFCost() 
    {
        return fCost;
    }

    public Vector3 getLocation() 
    {
        return location;
    }

    public PathNode(Vector3 location) 
    {
        this.location = location;
        hCost = 0;
        gCost = 0;
    }

    public void calculateFCost() 
    {
        fCost = gCost + hCost;
    }
}