using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Every cell has two walls that are its children gameobjects. When the maze is carved, every cell destroys one of its two walls
//Also contains the coordinates for the cell in a cell array. This data is used on the Recursive Backtracking algorithm.
public class Cell : MonoBehaviour {

    public GameObject wallNorth;
    public GameObject wallEast;
    public bool selectedInRecursiveBacktracking = false;
    public Vector2Int coordinatesOfCellInArray = new Vector2Int(0,0);
    
    public void DestroyWall(string targetWall)
    {
        if(targetWall.Equals("North"))
        {
            Destroy(wallNorth);
        }
        if(targetWall.Equals("East"))
        {
            Destroy(wallEast);
        }
    }
    
}
