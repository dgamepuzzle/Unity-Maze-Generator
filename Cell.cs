using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a cell of the grid
/// </summary>
//Every cell begins with an array containing data for two walls. The Maze Generation algorithm will inform the cell which wall to delete
//Also contains the coordinates for the cell in a cell array.

public class Cell : MonoBehaviour
{
    [SerializeField]
    private GameObject wall;
    private List<string> wallsBelongingToCell = new List<string> { "North", "East" };

    ///<doc>Used in the Recursive Backtracking algorithm, to show if the cell has been selected yet</doc>
    [HideInInspector]
    public bool selectedInRecursiveBacktracking = false;
    [HideInInspector]
    public Vector2Int coordinatesOfCellInGridArray = new Vector2Int(0, 0);

    /// <summary>
    /// Removes a wall from the cell's wall List
    /// </summary>
    /// <param name="targetWall">The direction of the wall to be removed. Either "North" or "East"</param>
    public void RemoveWallFromCell(string targetWall)
    {
        for (int i = 0; i < wallsBelongingToCell.Count; i++)
        {
            if (wallsBelongingToCell[i].Equals(targetWall))
            {
                wallsBelongingToCell.Remove(wallsBelongingToCell[i]);
            }
        }

    } 

    /// <summary>
    /// The cell instantiates the wall GameObjects that belong to it, according to the cell's wall List
    /// </summary>
    /// The walls will be children of the cell, and will rotate accordingly
    public void BuildWalls()
    {
        foreach (string wallDirection in wallsBelongingToCell)
        {
            GameObject newWall = GameObject.Instantiate(wall);
            newWall.transform.parent = transform;
            newWall.transform.localPosition = new Vector3(0.5f, 0.5f, 0);
            if (wallDirection.Equals("North"))
            {
                newWall.transform.eulerAngles = new Vector3(0, 0, -90);
            }
        }
    }
}
