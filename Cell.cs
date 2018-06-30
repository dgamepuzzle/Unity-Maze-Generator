using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Every cell has two walls that are its children gameobjects. When the maze is carved, every cell destroys one of its two walls
//Also contains the coordinates for the cell in a cell array. This data is used on the Recursive Backtracking algorithm.
public class Cell : MonoBehaviour
{
    [SerializeField]
    private GameObject wall;
    private List<string> wallsBelongingToCell = new List<string> { "North", "East" };

    [HideInInspector]
    public bool selectedInRecursiveBacktracking = false;
    [HideInInspector]
    public Vector2Int coordinatesOfCellInArray = new Vector2Int(0, 0);

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
