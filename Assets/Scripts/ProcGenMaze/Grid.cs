using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid 
{
    private int width;
    private int height;
    private int cellSize;
    private CellType[,] gridMap;

    public CellType[,] getGrid() 
    {
        return gridMap;
    }

    public Grid(int width, int height, int cellSize) 
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        createGrid();
    }

    private void createGrid() 
    {
        gridMap = new CellType[width, height];
        
        for (int i = 0; i < gridMap.GetLength(0); i++) 
        {
            for (int j = 0; j < gridMap.GetLength(1); j++) 
            {
                //Vertical lines
                Debug.DrawLine(new Vector3(i, 0, j) * cellSize, new Vector3(i, 0, j + 1) * cellSize, Color.white, 200f);
                //Horizontal lines
                Debug.DrawLine(new Vector3(i, 0, j) * cellSize, new Vector3(i + 1, 0, j) * cellSize, Color.white, 200f);
                
                gridMap[i, j] = CellType.None;
            }
        }

        Debug.DrawLine(new Vector3(0, 0, height) * cellSize, new Vector3(width, 0, height) * cellSize, Color.white, 200f);
        Debug.DrawLine(new Vector3(width, 0, 0) * cellSize, new Vector3(width, 0, height) * cellSize, Color.white, 200f);
        
    }
}
