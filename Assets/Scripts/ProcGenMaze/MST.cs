using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MST
{
    public static Dictionary<int, List<int>> AdjacencyList(List<RoomEdge> roomEdges) 
    {
        Dictionary<int, List<int>> adjList = new Dictionary<int, List<int>>();

        for (int i = 0; i < roomEdges.Count; i++) 
        {
            RoomEdge re = roomEdges[i];
            int id = re.getRoom1().getId();
            int id2 = re.getRoom2().getId();

            if (!adjList.ContainsKey(id))
            {
                List<int> nodes = new List<int>();
                nodes.Add(id2);
                adjList.Add(id, nodes);
            }
            else 
            {
                List<int> nodes = adjList[id];
                nodes.Add(id2);
                adjList[id] = nodes;
            }

            if (!adjList.ContainsKey(id2))
            {
                List<int> nodes = new List<int>();
                nodes.Add(id);
                adjList.Add(id2, nodes);
            }
            else
            {
                List<int> nodes = adjList[id2];
                nodes.Add(id);
                adjList[id2] = nodes;
            }
        }

        return adjList;
    }

    public static Dictionary<string, RoomEdge> DictRoomEdges(List<RoomEdge> roomEdges) 
    {
        Dictionary<string, RoomEdge> dictRoomEdges = new Dictionary<string, RoomEdge>();

        for (int i = 0; i < roomEdges.Count; i++) 
        {
            RoomEdge re = roomEdges[i];
            string id = re.getRoom1().getId() + "-" + re.getRoom2().getId();
            string id2 = re.getRoom2().getId() + "-" + re.getRoom1().getId();
            dictRoomEdges.Add(id, re);
            dictRoomEdges.Add(id2, re);
        }

        return dictRoomEdges;
    }

    public static bool elemInList(List<int> l, int elem) 
    {
        for (int i = 0; i < l.Count; i++) 
        {
            if (l[i] == elem)
                return true;
        }

        return false;
    }

    public static List<RoomEdge> MinSpanningTree(List<RoomEdge> roomEdges) 
    {
        Dictionary<int, List<int>> adjList = AdjacencyList(roomEdges);
        Dictionary<string, RoomEdge> dictRE = DictRoomEdges(roomEdges);

        List<int> visitedNodes = new List<int>();
        List<RoomEdge> mstree = new List<RoomEdge>();

        int lastNode = -1; //id of first room
        int countPos = 0;

        //List<int> keyList = new List<int>(adjList.Keys);
        //for (int s = 0; s < keyList.Count; s++)
        //    Debug.Log("adjlist node id: " + keyList[s]);

        visitedNodes.Add(lastNode);

        while (visitedNodes.Count != adjList.Keys.Count) 
        {
            List<int> nodes = adjList[lastNode];
            bool nodeFound = false;
            for (int i = 0; i < nodes.Count; i++)
            {
                if (!elemInList(visitedNodes, nodes[i])) //(!visitedNodes.Contains(nodes[i]))
                {
                    RoomEdge re = dictRE[lastNode + "-" + nodes[i]];
                    mstree.Add(re);
                    dictRE.Remove(lastNode + "-" + nodes[i]);
                    visitedNodes.Add(nodes[i]);
                    lastNode = nodes[i];
                    nodeFound = true;
                    break;
                }
            }
            if (!nodeFound) 
            {
                lastNode = visitedNodes[countPos];
                countPos++;
            }
        }


        //for(int s = 0; s < visitedNodes.Count; s++)
        //    Debug.Log("visited node id: " + visitedNodes[s]);

        //Drawing lines
        //for (int i = 0; i < mstree.Count; i++)
        //{
        //    Debug.Log("mst " + mstree[i].getRoom1().getId() + " " + mstree[i].getRoom2().getId());
        //    Debug.DrawLine(mstree[i].getRoom1().getLocation() * 2, mstree[i].getRoom2().getLocation() * 2, new Color(0f, 1f, 0f), 200f);
        //}
        

        //Adding cycles 
        Random random = new Random();
        foreach (var item in dictRE)
        {
            int value = random.Next(0, 10);
            if (value < 4) 
            {
                mstree.Add(item.Value);
            }
        }

        //Drawing lines
        //for (int i = 0; i < mstree.Count; i++)
        //{
            //Debug.Log("mst " + mstree[i].getRoom1().getId() + " " + mstree[i].getRoom2().getId());
            //Debug.DrawLine(mstree[i].getRoom1().getLocation() * 2, mstree[i].getRoom2().getLocation() * 2, new Color(0f, 1f, 0f), 200f);
        //}
        
        return mstree;
    }
}