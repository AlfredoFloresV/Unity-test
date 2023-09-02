using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    [SerializeField] MazeGenerator mazeGenerator;
    [SerializeField] GameObject mazeCellPrefab;

    public float CellSize = 1f;

    private void Start()
    {
        MazeCell[,] maze = mazeGenerator.GetMaze();

        for (int x = 0; x < mazeGenerator.mazeWidth; x++) {
            for (int y = 0; y < mazeGenerator.mazeHeight; y++) { 
                GameObject newCell = Instantiate(mazeCellPrefab, 
                                                 new Vector3((float)x * CellSize, 0f, (float)y * CellSize),
                                                 Quaternion.identity,
                                                 transform);

                newCell.transform.localScale = new Vector3( newCell.transform.localScale.x * CellSize,
                                                            newCell.transform.localScale.y * CellSize,
                                                            newCell.transform.localScale.z * CellSize
                                                          );

                MazeCellObject mazeCell = newCell.GetComponent<MazeCellObject>();

                bool top = maze[x, y].topWall;
                bool left = maze[x, y].leftWall;

                bool right = false;
                bool bottom = false;


                if (x == mazeGenerator.mazeWidth - 1) right = true;
                if (y == 0) bottom = true;

                mazeCell.init(top, bottom, right, left);
            }
        }

    }
}
