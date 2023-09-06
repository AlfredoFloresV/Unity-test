using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class StartGrid : MonoBehaviour
{
    [SerializeField]
    [Range(5,50)]
    private int width = 5;

    [SerializeField]
    [Range(5, 50)]
    private int height = 5;

    [SerializeField]
    private int scale = 1;

    [SerializeField]
    private GameObject roomPrefab;

    private CellType[,] grid;

    // Start is called before the first frame update
    void Start()
    {
        createMap();
    }

    private void createMap() 
    {
        Random random = new Random();
        Grid gridObj = new Grid(width, height, 1);
        grid = gridObj.getGrid();

        for (int i = 0; i < grid.GetLength(0); i++) 
        {
            for (int j = 0; j < grid.GetLength(1); j++) 
            {
                if (random.Next(1, 10) % 2 == 0) 
                {
                    Vector3 location = new Vector3(random.Next(0, width), 0, random.Next(0, height));
                    Vector3 size = new Vector3(random.Next(2, 4), 3f, random.Next(2, 4)) * scale;
                    grid[i, j] = CellType.Room;
                    DrawRoom(location, size);
                }
                else
                {
                    grid[i, j] = CellType.None;
                }
            }
        }
    }

    private void DrawRoom(Vector3 location, Vector3 size)
    {
        GameObject roomObj = Instantiate(roomPrefab, location + size * 0.5f, Quaternion.identity);
        roomObj.GetComponent<Transform>().localScale = size;


        BoundsInt bounds = new BoundsInt(Vector3Int.FloorToInt(location), Vector3Int.FloorToInt(size));
        
        foreach (var pos in bounds.allPositionsWithin)
        {
            grid[pos.x, pos.z] = CellType.Room;
        }
    }
}

