﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Calculates the Maze's wall by applying the Binary Tree Algorithm
/// </summary>
/*
 * For every Cell, the maze generator will randomly decide if it will destroy the north or east wall of the cell.</para>
 * If the cell is located in the top row of the maze, it will alway break it's east wall
 * If the cell is located in the right column of the maze it will always break the north wall</para>
*/

public class BinaryTreeAlgorithm : MazeGeneration
{
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
                    cellArray[column, row].RemoveWallFromCell("North");
                }
                else if (((randomWallToBreak == 1) && (column != gridSizeX - 1)) || (row == gridSizeY - 1))
                {
                    cellArray[column, row].RemoveWallFromCell("East");
                }

            }
            //yields after a row has been completed
            yield return null;
        }
        BuildMazeWalls();

    }
}
