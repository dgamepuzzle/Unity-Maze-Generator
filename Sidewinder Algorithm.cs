using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Calculates the Maze's wall by applying the Sidewinder Algorithm
/// </summary>
//For every Cell, the maze generator will randomly decide if it will destroy the north or east wall of the cell
//When a cell destroys a wall, it adds that cell on a cell list. 
//When a cell is about to destroy a north wall, a random cell from the list is picked and that cell destroys its north wall instead. Then the list is emptied.
public class SidewinderAlgorithm : MazeGeneration
{

    private List<Cell> cellList = new List<Cell>();

    protected override IEnumerator CalculateMazeWalls()
    {
        for (int row = 0; row < gridSizeY; row++)
        {
            for (int column = 0; column < gridSizeX; column++)
            {
                //randomWallToBreak : 0 = "North" , 1 = "East"
                int randomWallToBreak = Random.Range(0, 2);
                if (((randomWallToBreak == 0) && (row != gridSizeY - 1)) || (column == gridSizeX - 1))
                {
                    cellList.Add(cellArray[column, row]);
                    int randomCell = Random.Range(0, cellList.Count);
                    cellList[randomCell].RemoveWallFromCell("North");
                    cellList.Clear();
                }
                else if (((randomWallToBreak == 1) && (column != gridSizeX - 1)) || (row == gridSizeY - 1))
                {
                    cellArray[column, row].RemoveWallFromCell("East");
                    cellList.Add(cellArray[column, row]);
                }
            }
            //yields after a row has been completed
            yield return null;
        }
        BuildMazeWalls();
    }
}
