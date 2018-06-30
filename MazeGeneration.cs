using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGeneration : MonoBehaviour
{
    protected Cell[,] cellInTheGrid = new Cell[MazeProperties.MazeWidth, MazeProperties.MazeHeight];

    protected GameObject cellGameObject;
    protected float gridSizeX, gridSizeY;

    //Fills the grid with Cells
    public void GenerateMaze(GameObject cellGameObject,float gridSizeX, float gridSizeY)
    {
        this.cellGameObject = cellGameObject;
        this.gridSizeX = gridSizeX;
        this.gridSizeY = gridSizeY;

        CreateMazeCells();
        StartCoroutine(CalculateMazeWalls());

    }

    private void CreateMazeCells()
    {
        Vector3 cellPosition = new Vector3((-(float)gridSizeX / 2) + 0.5f, (-(float)gridSizeY / 2) + 0.5f, 0);
        for (int row = 0; row < gridSizeY; row++)
        {
            for (int column = 0; column < gridSizeX; column++)
            {
                Cell newCell = Instantiate(cellGameObject, cellPosition, transform.rotation).GetComponent<Cell>();
                cellInTheGrid[column, row] = newCell;
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
        foreach (Cell mazeCell in cellInTheGrid)
        {
            mazeCell.BuildWalls();
        }
    }


}
