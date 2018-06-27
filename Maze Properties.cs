using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Holds values selected by the user in the main menu, after he presses the "Generate Maze" button, so they can be used in any scene
public static class MazeProperties 
{
    private static int mazeHeight;
    public static int MazeHeight
    {
        get
        {
            return mazeHeight;
        }
        set
        {
            mazeHeight = value;
        }
    }
    private static int mazeWidth;
    public static int MazeWidth
    {
        get
        {
            return mazeWidth;
        }
        set
        {
            mazeWidth = value;
        }
    }
    private static string mazeGenerationAlgorithm;
    public static string MazeGenerationAlgorithm
    {
        get
        {
            return mazeGenerationAlgorithm;
        }
        set
        {
            mazeGenerationAlgorithm = value;
        }
    }


}
