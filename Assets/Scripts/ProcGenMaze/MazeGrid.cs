using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

using Random = System.Random;

public class MazeGrid : MonoBehaviour
{
    [SerializeField]
    private NavMeshSurface surface;

    [SerializeField]
    [Range(20, 80)]
    private int mazeSizeX;

    [SerializeField]
    [Range(20, 80)]
    private int mazeSizeY;

    [SerializeField]
    private int roomSize = 4;

    [SerializeField]
    private GameObject roomPrefab;

    [SerializeField]
    private GameObject hallwayPrefab;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject enemy;

    [SerializeField]
    private GameObject destPrefab;

    [SerializeField]
    private int Scale = 1;

    [SerializeField]
    private Material greenMaterial;

    private int roomCount;
    private List<Room> rooms;
    private List<RoomEdge> roomEdges;
    private List<Vector3> doorLocations;
    private List<Transform> destinations;
    private CellType[,] grid;
    private GameObject[,] prefabGrid;
    

    private void Start()
    {
        roomCount = (int) ((mazeSizeX + mazeSizeY) / 2.0f) + 3;
        rooms = new List<Room>();
        roomEdges = new List<RoomEdge>();
        doorLocations = new List<Vector3>();
        destinations = new List<Transform>();
        //Debug.Log("Generating " + roomCount + " rooms");

        prefabGrid = new GameObject[mazeSizeX, mazeSizeY];
        for (int x = 0; x < prefabGrid.GetLength(0); x++)
        {
            for (int z = 0; z < prefabGrid.GetLength(1); z++)
            {
                prefabGrid[x, z] = null;
            }
        }


        createSpecialRooms();
        createGrid(); // Imprimir el contenido de Grid
        DelaunayTriangulation();

        /*
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                Debug.Log(i + " " + j + " " + grid[i, j]);
            }
        }
        */

        CreateHallways();
        ValidateWalls();
        PlaceDoors();
        //testRoom();

        placeItems();
        surface.BuildNavMesh();

        NavMeshHit closestHit;
        if (NavMesh.SamplePosition(enemy.transform.position, out closestHit, 500f, NavMesh.AllAreas))
            enemy.transform.position = closestHit.position;
        else Debug.Log("something weird happened");
        enemy.GetComponent<LarryAI>().setRandomDestinations(destinations);
    }

    private void placeItems() 
    {
        //Rotar payasos
        int count = 0;
        Random random = new Random();
        while (count < 5)
        {
            int x = random.Next(0, mazeSizeX);
            int z = random.Next(0, mazeSizeY);

            if (grid[x, z] == CellType.Hallway)
            {
                bool failDist = false;
                Vector3 l = new Vector3((x + 0.1f) * Scale, -0.2f, (z + 0.1f) * Scale);

                for (int i = 0; i < destinations.Count; i++) 
                {
                    if (Vector3.Distance(l, destinations[i].transform.position) < 20f) 
                    {
                        failDist = true;
                    }
                }
                if (failDist == false) 
                {
                    GameObject dest = Instantiate(destPrefab, l, Quaternion.identity);
                    destinations.Add(dest.transform);
                    count++;
                }
            }
        }
    }


    private void updateGrids(Vector3 loc, Vector3 size, Room room) 
    {
        int count = 0;
        for (int x = 0; x < (int)size.x; x++)
        {
            for (int z = 0; z < (int)size.z; z++)
            {
                Vector3 realLoc = loc + new Vector3(x, 0, z);
                grid[(int)realLoc.x, (int)realLoc.z] = CellType.Room;
                prefabGrid[(int)realLoc.x, (int)realLoc.z] = room.getPrefabs()[count];
                count++;
            }
        }
    }

    private Room createRoom(Vector3 location, Vector3 size, int id, int scale)
    {
        List<GameObject> roomCubes = new List<GameObject>();

        for (int x = 0; x < (int) size.x; x++) 
        {
            for (int z = 0; z < (int) size.z; z++) 
            {
                Vector3 realLoc = location * scale + new Vector3(x * scale + (scale / 2), 0.5f * scale, z * scale + (scale / 2));
                GameObject roomObj = Instantiate(roomPrefab, realLoc, Quaternion.identity);
                roomObj.GetComponent<Transform>().localScale = new Vector3(1f * Scale, 1f * Scale, 1f * Scale);
                roomCubes.Add(roomObj);
            }
        }

        Room room = new Room(location, size, roomCubes, id, scale);

        return room;
    }

    private GameObject DrawHallway(Vector3 location, Vector3 size) 
    {
        //location + size * 0.5f
        GameObject hallwayObj = Instantiate(hallwayPrefab, location + new Vector3((Scale/2), 0, (Scale/2)), Quaternion.identity);
        hallwayObj.GetComponent<Transform>().localScale = size;
        return hallwayObj;
    }

    private void testRoom()
    {
        Random random = new Random();
        Grid gridObj = new Grid(mazeSizeX, mazeSizeY, Scale);
        grid = gridObj.getGrid();

        Vector3 loc = new Vector3(random.Next(1, mazeSizeX) * Scale, 0f, random.Next(1, mazeSizeY) * Scale);
        Vector3 size = new Vector3(random.Next(2, roomSize) * Scale, 1.0f * Scale, random.Next(2, roomSize) * Scale);

        Room room = createRoom(loc, size, 0, Scale);
    }


    private void createSpecialRooms() 
    {
        Grid gridObj = new Grid(mazeSizeX, mazeSizeY, Scale);
        grid = gridObj.getGrid();
        
        //First room
        Vector3 loc = new Vector3(2f, 0f, 2f);
        Vector3 size = new Vector3(2f, 1.0f, 4f);
        Room room = createRoom(loc, size, -1, Scale);
        rooms.Add(room);
        updateGrids(loc, size, room);
        
        //Second room
        loc = new Vector3(2f, 0f, (mazeSizeY - 4));
        size = new Vector3(4f, 1.0f, 2f);
        room = createRoom(loc, size, -2, Scale);
        rooms.Add(room);
        updateGrids(loc, size, room);

        //Third room
        loc = new Vector3((mazeSizeX - 4), 0f, (mazeSizeY - 8));
        size = new Vector3(2f, 1.0f, 4f);
        room = createRoom(loc, size, -3, Scale);
        rooms.Add(room);
        updateGrids(loc, size, room);

        //Fourth room
        loc = new Vector3((mazeSizeX - 8), 0f, 2f);
        size = new Vector3(4f, 1.0f, 2f);
        room = createRoom(loc, size, -4, Scale);
        rooms.Add(room);
        updateGrids(loc, size, room);


        //Determinar inicio y fin
        player.GetComponent<Transform>().position = new Vector3(6f, 1f, 6f);
    }

    private void createGrid() 
    {
        Random random = new Random();
        int offset = 1 * Scale;

        for (int i = 0; i < roomCount; i++) 
        {
            Vector3 loc = new Vector3(random.Next(1, mazeSizeX), 0f, random.Next(1, mazeSizeY));
            Vector3 size = new Vector3(random.Next(2, roomSize), 1.0f, random.Next(2, roomSize));

            //Debug.Log(loc + " " + size + " " +i);
            Room room = createRoom(loc, size, i, Scale);
            bool addRoom = true;

            //Intersect new room with the others
            for (int j = 0; j < rooms.Count; j++) 
            {
                Vector3 bounds = (loc + size);
                
                
                bool outOfBounds = (bounds.x >= mazeSizeX || bounds.z >= mazeSizeY);

                if (Room.isInsersecting(room, rooms[j], Scale) || outOfBounds) 
                {
                    //Debug.Log("cannot insert " + loc + " " + size);
                    //Delete prefabs
                    List<GameObject> prefabs = room.getPrefabs();
                    for (int k = 0; k < prefabs.Count; k++) 
                    {
                        Destroy(prefabs[k]);
                    }
                    addRoom = false;
                    break;
                }
            }

            if (addRoom) 
            {
                rooms.Add(room);
                updateGrids(loc, size, room);
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


    private bool existLocation(Vector3 location) 
    {
        for(int i = 0; i < doorLocations.Count; i++) 
        {
            Vector3 aux = doorLocations[i];
            if (Vector3.Distance(location, aux) < 1)
                return true;
        }

        return false;
    }

    private void CreateHallways()
    {
        List<RoomEdge> mstree = MST.MinSpanningTree(roomEdges);
        CellType[,] gridAux = grid;

        for (int k = 0; k < mstree.Count; k++) //mstree.Count; i++)
        {
            PathFinder pf = new PathFinder(gridAux, mazeSizeX, mazeSizeY);
            RoomEdge re = mstree[k];

            List<PathNode> path = pf.findPath(re.getRoom1().getLocation(), re.getRoom2().getLocation());
            //Debug.Log(path[0].getLocation() + " " + path[path.Count - 1].getLocation());

            if (!existLocation(path[0].getLocation()))
            {
                doorLocations.Add(path[0].getLocation());
            }
            if (!existLocation(path[path.Count - 1].getLocation())) 
            {
                doorLocations.Add(path[path.Count - 1].getLocation());
            }
            
            gridAux = pf.getGrid();
        }

        //Update grid
        Vector3 size = new Vector3(1f, 1f, 1f) * Scale;
        for (int i = 0; i < gridAux.GetLength(0); i++)
        {
            for (int j = 0; j < gridAux.GetLength(1); j++)
            {
                //Debug.Log(newGrid[i,j]);
                if (gridAux[i, j] == CellType.Hallway && prefabGrid[i, j] == null)
                {
                    Vector3 location = new Vector3(i * Scale, 0.5f * Scale, j * Scale);
                    GameObject hallwayObj = DrawHallway(location, size);
                    grid[i, j] = CellType.Hallway;
                    prefabGrid[i, j] = hallwayObj;
                }
                else 
                {
                    if ((grid[i, j] == CellType.Room && (prefabGrid[i, j] == null)) || (gridAux[i, j] == CellType.Hallway && prefabGrid[i, j] != null)) 
                    {
                        Debug.Log("Celda rara " + i + " " + j + " " + grid[i, j] + " " + (prefabGrid[i, j] == null));
                    }
                }
            }
        }
    }


    private bool validNeighbour(int x1, int z1, int x2, int z2) 
    {
        if (grid[x1, z1] == CellType.Room && grid[x2, z2] == CellType.Hallway)
            return false;
        if (grid[x1, z1] == CellType.Hallway && grid[x2, z2] == CellType.Room)
            return false;
        return true;
    }

    private void ValidateWalls() 
    {
        for (int x = 0; x < prefabGrid.GetLength(0); x++) 
        {
            for (int z = 0; z < prefabGrid.GetLength(1); z++) 
            {
                if (prefabGrid[x, z] == null) continue;

                CellType myType = grid[x, z];
                
                prefabGrid[x,z].GetComponent<MazeCellObject>().DisableDoor();
                
                //Check Left
                if (x - 1 > 0 && prefabGrid[x - 1, z] != null && validNeighbour(x,z,x-1,z)) 
                {
                    prefabGrid[x - 1, z].GetComponent<MazeCellObject>().DisableRight();
                    prefabGrid[x, z].GetComponent<MazeCellObject>().DisableLeft();
                }
                //Check right
                if (x + 1 < mazeSizeX && prefabGrid[x + 1, z] != null && validNeighbour(x, z, x + 1, z))
                {
                    prefabGrid[x + 1, z].GetComponent<MazeCellObject>().DisableLeft();
                    prefabGrid[x, z].GetComponent<MazeCellObject>().DisableRight();
                }
                //Check Top
                if (z + 1 < mazeSizeY && prefabGrid[x, z + 1] != null && validNeighbour(x, z, x, z + 1))
                {
                    prefabGrid[x, z + 1].GetComponent<MazeCellObject>().DisableBottom();
                    prefabGrid[x, z].GetComponent<MazeCellObject>().DisableTop();
                }
                //Check Bottom
                if (z - 1 > 0 && prefabGrid[x, z - 1] != null && validNeighbour(x, z, x, z - 1))
                {
                    //Debug.Log(grid[x, z] + " " + x + " " + z + " - " + x + " " + (z - 1));
                    prefabGrid[x, z - 1].GetComponent<MazeCellObject>().DisableTop();
                    prefabGrid[x, z].GetComponent<MazeCellObject>().DisableBottom();
                }
            }
        }
    }

    private void PlaceDoors()
    {
        for (int i = 0; i < doorLocations.Count; i++) 
        {
            Vector3 loc = doorLocations[i];
            int x = (int) loc.x;
            int z = (int) loc.z;
            //Debug.Log(grid[x, z] + " " + loc);

            CellType cell = grid[x, z];
            CellType lookfor = CellType.None;

            switch (cell) 
            {
                case CellType.Hallway:
                    lookfor = CellType.Room;
                    break;
                case CellType.Room:
                    lookfor = CellType.Hallway;
                    break;
                default:
                    lookfor = CellType.None;
                    break;
            }

            //Right
            if (x + 1 < mazeSizeX && grid[x + 1, z] == lookfor) 
            {
                prefabGrid[x, z].GetComponent<MazeCellObject>().DisableRight();
                prefabGrid[x + 1, z].GetComponent<MazeCellObject>().DisableLeft();
                prefabGrid[x + 1, z].GetComponent<MazeCellObject>().placeDoor("left");
            }

            //Left
            if (x - 1 < mazeSizeX && grid[x - 1, z] == lookfor)
            {
                prefabGrid[x, z].GetComponent<MazeCellObject>().DisableLeft();
                prefabGrid[x - 1, z].GetComponent<MazeCellObject>().DisableRight();
                prefabGrid[x - 1, z].GetComponent<MazeCellObject>().placeDoor("right");
            }

            //Top
            if (z + 1 < mazeSizeY && grid[x, z + 1] == lookfor)
            {
                if ( (x - 1 > 0 && grid[x - 1, z + 1] != CellType.Hallway) ||
                     (x + 1 < mazeSizeX && grid[x + 1, z + 1] != CellType.Hallway))
                {
                    prefabGrid[x, z].GetComponent<MazeCellObject>().DisableTop();
                    prefabGrid[x, z + 1].GetComponent<MazeCellObject>().DisableBottom();
                    prefabGrid[x, z + 1].GetComponent<MazeCellObject>().placeDoor("bottom");
                }
            }

            //Bottom
            if (z - 1 > 0 && grid[x, z - 1] == lookfor)
            {
                if ((x - 1 > 0 && grid[x - 1, z - 1] != CellType.Hallway) ||
                     (x + 1 < mazeSizeX && grid[x + 1, z - 1] != CellType.Hallway)) 
                {
                    prefabGrid[x, z].GetComponent<MazeCellObject>().DisableBottom();
                    prefabGrid[x, z - 1].GetComponent<MazeCellObject>().DisableTop();
                    prefabGrid[x, z - 1].GetComponent<MazeCellObject>().placeDoor("top");
                }     
            }
        }
    }

    /*
    private void PlaceDoors()
    {
        // No puede haber doors pegadas, validar vecinos acorde a la dirección y al maxroomsize
        //for (int i = 0; i < rooms.Count; i++) 
        //{
        //    Debug.Log(rooms[i].getLocation() + " " + rooms[i].getId());
        //}

        //Look for room wall connected with hallways
        for (int x = 0; x < prefabGrid.GetLength(0); x++)
        {
            for (int z = 0; z < prefabGrid.GetLength(1); z++)
            {
                if (grid[x, z] != CellType.Room) continue;

                //Check Top create bottom door from hallway
                if (z + 1 < mazeSizeY && grid[x, z + 1] == CellType.Hallway) 
                {
                    prefabGrid[x, z + 1].GetComponent<MazeCellObject>().placeDoor("bottom");
                }

                //Check Bottom create top door from hallway
                if (z - 1 > 0 && grid[x, z - 1] == CellType.Hallway)
                {
                    prefabGrid[x, z - 1].GetComponent<MazeCellObject>().placeDoor("top");
                }

                //Check Left create right door from hallway
                if (x - 1 > 0 && grid[x - 1, z] == CellType.Hallway) 
                {
                    prefabGrid[x - 1, z].GetComponent<MazeCellObject>().placeDoor("right");
                }

                //Check Left create right door from hallway
                if (x + 1 < mazeSizeY && grid[x + 1, z] == CellType.Hallway)
                {
                    prefabGrid[x + 1, z].GetComponent<MazeCellObject>().placeDoor("left");
                }
            }
        }
    }
    */
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
    private List<GameObject> prefabs;
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

    public List<GameObject> getPrefabs() 
    {
        return prefabs;
    }

    public BoundsInt getBounds() 
    {
        return bounds;
    }

    public static bool isInsersecting(Room room1, Room room2, int scale) 
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

    public Room(Vector3 loc, Vector3 s, List<GameObject> prefabs, int i, int scale) 
    {
        this.prefabs = prefabs;
        size = s;
        bounds = new BoundsInt(Vector3Int.FloorToInt(loc), Vector3Int.FloorToInt(size));
        id = i;
        this.scale = scale;
        location = loc;

        //Modify location point to place the door
        /*
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
        */
    }
}

public enum CellType
{
    None,
    Room,
    Hallway,
    Aux,
    Door
}