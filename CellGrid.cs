using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Sets up the outter wall of the grid, and calls an algorithm to calculate the maze
 */
public class CellGrid : MonoBehaviour
{
    [SerializeField]
    private GameObject cellGameObject;
    [SerializeField]
    private Transform[] outerWalls = new Transform[4];
    private int gridSizeX, gridSizeY;
    ///<doc>An array containing every Cell in the grid, with the same coordinates as it has on the grid</doc>

    private void Start()
    {
        gridSizeX = MazeProperties.MazeWidth;
        gridSizeY = MazeProperties.MazeHeight;
        foreach (Transform wall in outerWalls)
        {
            GenerateOuterWall(wall);
        }
        string mazeGenerationAlgorithm = MazeProperties.MazeGenerationAlgorithm;
        if(mazeGenerationAlgorithm.Equals("Binary Tree"))
        {
            BinaryTreeAlgorithm algorithm = gameObject.AddComponent<BinaryTreeAlgorithm>();
            algorithm.GenerateMaze(cellGameObject,gridSizeX, gridSizeY);
        }
        else if (mazeGenerationAlgorithm.Equals("Sidewinder"))
        {
            SidewinderAlgorithm algorithm = gameObject.AddComponent<SidewinderAlgorithm>();
            algorithm.GenerateMaze(cellGameObject, gridSizeX, gridSizeY);
        }
        else if (mazeGenerationAlgorithm.Equals("Recursive Backtracking"))
        {
            RecursiveBacktrackingAlgorithm algorithm = gameObject.AddComponent<RecursiveBacktrackingAlgorithm>();
            algorithm.GenerateMaze(cellGameObject, gridSizeX, gridSizeY);
        }
    }

    //Moves the outer walls of the grid and scales them as necessary
    private void GenerateOuterWall(Transform wallTransform)
    {
        //In order for this to work, the walls must be in the scene, in the positions (1,0,0) (-1,0,0) (0,1,0) (0,-1,0)
        wallTransform.position =Vector3.Scale(wallTransform.position,new Vector3((float)gridSizeX / 2, (float)gridSizeY / 2, 0));
        //If the position.x of the wallTransform is 0, then it is either an East or West wall, otherwise it's a South or North wall.Each wall scales accordingly
        if (wallTransform.position.x != 0)
            wallTransform.localScale = new Vector3(wallTransform.localScale.x, gridSizeY, 1);
        else
            wallTransform.localScale = new Vector3(wallTransform.localScale.x, gridSizeX, 1);
        
    }

}
