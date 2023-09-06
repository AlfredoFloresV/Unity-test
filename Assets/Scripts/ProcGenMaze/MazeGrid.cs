using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MazeGrid : MonoBehaviour
{
    [SerializeField]
    [Range(3,50)]
    private int mazeSizeX;

    [SerializeField]
    [Range(3, 50)]
    private int mazeSizeY;

    [SerializeField]
    private int roomSize = 4;

    [SerializeField]
    private GameObject roomPrefab;

    [SerializeField]
    private GameObject hallwayPrefab;

    [SerializeField]
    private int Scale = 1;

    [SerializeField]
    private Material greenMaterial;

    private int roomCount;
    private List<Room> rooms;
    private List<RoomEdge> roomEdges;
    private CellType[,] grid;


    private void Start()
    {
        roomCount = (int) ((mazeSizeX + mazeSizeY) / 2.0f) + 3;
        rooms = new List<Room>();
        roomEdges = new List<RoomEdge>();
        Debug.Log("Generating " + roomCount + " rooms");
        createGrid();
        DelaunayTriangulation();
        CreateHallways();
    }

    private Room createRoom(Vector3 location, Vector3 size, int id, int scale)
    {
        GameObject roomObj = Instantiate(roomPrefab, location + size * 0.5f, Quaternion.identity);
        roomObj.GetComponent<Transform>().localScale = size;
        //go.GetComponent<MeshRenderer>().material = material;
        Room room = new Room(location, size, roomObj, id, scale);

        return room;
    }

    private void DrawHallway(Vector3 location, Vector3 size) 
    {
        GameObject hallwayObj = Instantiate(hallwayPrefab, location + size * 0.5f, Quaternion.identity);
        hallwayObj.GetComponent<Transform>().localScale = size;
    }

    private void createGrid() 
    {
        Random random = new Random();
        Grid gridObj = new Grid(mazeSizeX, mazeSizeY, Scale);
        grid = gridObj.getGrid();

        int offset = 1 * Scale;

        for (int i = 0; i < roomCount; i++) 
        {
            Vector3 loc = new Vector3(random.Next(1, mazeSizeX) * Scale, 0f, random.Next(1, mazeSizeY) * Scale);
            Vector3 size = new Vector3(random.Next(2, roomSize) * Scale, 3.0f, random.Next(2, roomSize) * Scale);

            Debug.Log(loc + " " + size + " " +i);
            Room room = createRoom(loc, size, i, Scale);
            bool addRoom = true;

            for (int j = 0; j < rooms.Count; j++) 
            {
                bool outOfBounds = room.getLocation().x > (mazeSizeX * Scale - size.x - 1 * Scale) || 
                                   room.getLocation().z > (mazeSizeY * Scale - size.z - 1 * Scale);

                if (Room.isInsersecting(room, rooms[j], offset) || outOfBounds) {
                    Destroy(room.getPrefab());
                    addRoom = false;
                    break;
                }
            }

            if (addRoom) 
            {
                rooms.Add(room);
                BoundsInt bounds = room.getBounds();

                foreach (var pos in bounds.allPositionsWithin)
                {
                    //Debug.Log(pos.x + " " + pos.z);
                    if (pos.x / Scale < mazeSizeX && pos.z / Scale < mazeSizeY) 
                    {
                        //Debug.Log("Setting room with location " + room.getLocation() + " in grid " + pos.x + " " + pos.z);
                        grid[pos.x / Scale, pos.z / Scale] = CellType.Room;
                    }
                }

            }
        }
    }

    private void DelaunayTriangulation() 
    {
        for (int i = 0; i < rooms.Count; i++) 
        {
            for (int j = i + 1; j < rooms.Count; j++) 
            {
                RoomEdge re = new RoomEdge(rooms[i], rooms[j]);
                bool addEdge = true;

                //Debug.Log("working with edge " + re.getRoom1().getId() + " " + re.getRoom2().getId());
                for (int k = 0; k < roomEdges.Count; k++)
                {
                    if (RoomEdge.intersects(re, roomEdges[k])) 
                    {
                        if (re.getDistance() < roomEdges[k].getDistance())
                        {
                            //Debug.Log("removing crossed edge from rooms " + roomEdges[k].getRoom1().getId() + " " + roomEdges[k].getRoom2().getId());
                            roomEdges.RemoveAt(k);
                        }
                        else 
                        {
                            //Debug.Log("ignoring new edge");
                            addEdge = false;
                            break;
                        }
                    }
                }

                if (addEdge == true) 
                {
                    roomEdges.Add(re);
                }
            }
        }

        //Drawing lines
        //for (int i = 0; i < roomEdges.Count; i++) 
        //{
        //    Debug.DrawLine(roomEdges[i].getRoom1().getLocation(), roomEdges[i].getRoom2().getLocation(), new Color(0f,0f,1f), 120f);
        //}
    }


    
    private void CreateHallways()
    {
        List<RoomEdge> mstree = MST.MinSpanningTree(roomEdges);
        

        for (int k = 0; k < mstree.Count; k++) //mstree.Count; i++)
        {
            PathFinder pf = new PathFinder(grid, mazeSizeX, mazeSizeY);
            RoomEdge re = mstree[k];
            pf.findPath(re.getRoom1().getScaledLocation(), re.getRoom2().getScaledLocation());

            grid = pf.getGrid();

            //Update grid
            Vector3 size = new Vector3(1f, 1f, 1f) * Scale;
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    //Debug.Log(newGrid[i,j]);
                    if (grid[i, j] == CellType.Hallway)
                    {
                        Vector3 location = new Vector3(i * Scale, 0, j * Scale);
                        DrawHallway(location, size);
                    }
                }
            }
        }
    }


}


    // Used for triangulation
    public class RoomEdge 
{
    private Room room1;
    private Room room2;
    private float distance;
    private Vector3 p1; 
    private Vector3 p2;

    public Room getRoom1() 
    {
        return room1;
    }

    public Room getRoom2()
    {
        return room2;
    }

    public float getDistance() 
    {
        return distance;
    }

    public Vector3 getP1()
    {
        return p1;
    }

    public Vector3 getP2() 
    {
        return p2;
    }

    public RoomEdge(Room r1, Room r2) 
    {
        room1 = r1;
        room2 = r2;
        distance = Vector3.Distance(room1.getLocation(), room2.getLocation());
        p1 = room1.getLocation();
        p2 = room2.getLocation();
    }

    public static bool intersects(RoomEdge edge1, RoomEdge edge2) 
    {
        ParametricEquation eq1 = new ParametricEquation(edge1.getP1(), edge1.getP2());
        ParametricEquation eq2 = new ParametricEquation(edge2.getP1(), edge2.getP2());

        return SystemEquation.intersects(eq1, eq2);
    }
}



// Essential class for rendering a room
public class Room 
{
    private GameObject objPrefab;
    private Vector3 location;
    private Vector3 size;
    private BoundsInt bounds;
    private int id;
    private int scale;

    public int getId() 
    {
        return id;
    }

    public Vector3 getLocation() 
    {
        return location;
    }

    public Vector3 getScaledLocation() 
    {
        return location / scale;
    }

    public Vector3 getSize() 
    {
        return size;
    }

    public GameObject getPrefab() 
    {
        return objPrefab;
    }

    public BoundsInt getBounds() 
    {
        return bounds;
    }

    public static bool isInsersecting(Room room1, Room room2, int offset) 
    {
        foreach (var pos1 in room1.bounds.allPositionsWithin)
        {
            foreach (var pos2 in room2.bounds.allPositionsWithin) 
            {
                if (Vector3.Distance(pos1, pos2) < 3) 
                {
                    return true;
                }
            }
        }

        return false;
    }

    public Room(Vector3 loc, Vector3 s, GameObject prefab, int i, int scale) 
    {
        objPrefab = prefab;
        size = s;
        bounds = new BoundsInt(Vector3Int.FloorToInt(loc), Vector3Int.FloorToInt(size));
        id = i;
        this.scale = scale;

        Random random = new Random();
        int num = random.Next(1,10);

        if (num % 2 == 0)
        {
            location = loc + new Vector3(1f * scale, 0f, 0f);
        }
        else 
        {
            location = loc + new Vector3(0f, 0f, 1f * scale); ;
        }
    }
}

public enum CellType
{
    None,
    Room,
    Hallway,
    Aux
}