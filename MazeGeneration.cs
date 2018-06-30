using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fills the grid with Cells, calculates the maze's walls and builds the maze
/// </summary>

/*
 * Every maze generation algorithm inherits from MazeGeneration, and overrides the CalculateMazeWalls() method
 */
public class MazeGeneration : MonoBehaviour
{
    ///<doc>An array containing every cell in the grid, in the same coordinates as in the grid</doc>
    protected Cell[,] cellArray = new Cell[MazeProperties.MazeWidth, MazeProperties.MazeHeight];

    protected GameObject cellGameObject;
    protected float gridSizeX, gridSizeY;

    
    public void GenerateMaze(GameObject cellGameObject,float gridSizeX, float gridSizeY)
    {
        this.cellGameObject = cellGameObject;
        this.gridSizeX = gridSizeX;
        this.gridSizeY = gridSizeY;

        CreateMazeCells();
        StartCoroutine(CalculateMazeWalls());

    }

    /// <summary>
    /// Fills the grid with Cells
    /// </summary>
    private void CreateMazeCells()
    {
        Vector3 cellPosition = new Vector3((-(float)gridSizeX / 2) + 0.5f, (-(float)gridSizeY / 2) + 0.5f, 0);
        for (int row = 0; row < gridSizeY; row++)
        {
            for (int column = 0; column < gridSizeX; column++)
            {
                Cell newCell = Instantiate(cellGameObject, cellPosition, transform.rotation).GetComponent<Cell>();
                cellArray[column, row] = newCell;
                newCell.coordinatesOfCellInGridArray = new Vector2Int(column, row);
                cellPosition.x = cellPosition.x + 1;
            }
            cellPosition.y = cellPosition.y + 1;
            //Points to the start of the Row
            cellPosition.x = (-(float)gridSizeX / 2) + 0.5f;
        }
    }

    protected virtual IEnumerator CalculateMazeWalls()
    {
        yield return null;
    }

    /// <summary>
    /// Informs each cell build the walls that correspond to it
    /// </summary>
    protected void BuildMazeWalls()
    {
        foreach (Cell mazeCell in cellArray)
        {
            mazeCell.BuildWalls();
        }
    }


}
