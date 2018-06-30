using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Creates and fills the maze's grid with Cells, and carves the maze's walls depending on the algorithm selected
//Also manipulates the camera to fit the whole maze in the screen
public class CellGrid : MonoBehaviour
{
    public GameObject Cell;
    public Transform[] outerWalls = new Transform[4];
    private int gridSizeX, gridSizeY;
    //Used in the Recursive Backtracking algorithm. Contains every created cell
    private Cell[,] cellInTheGrid = new Cell[(int)MazeProperties.MazeWidth,(int)MazeProperties.MazeHeight];
    //Used in the Sidewinder algorithm. Contains the set of cells that may destroy their north wall
    List<Cell> sidewinderCellSetList = new List<Cell>();

    public void Start()
    {
        gridSizeX = MazeProperties.MazeWidth;
        gridSizeY = MazeProperties.MazeHeight;
        foreach (Transform wall in outerWalls)
        {
            GenerateOuterWall(wall);
        }
        GenerateMaze();
        BuildMazeWalls();
    }

    //Moves the outer walls of the grid and scales them as necessary
    public void GenerateOuterWall(Transform wallTransform)
    {
        //In order for this to work, the walls must be in the scene, in the positions (1,0,0) (-1,0,0) (0,1,0) (0,-1,0)
        wallTransform.position =Vector3.Scale(wallTransform.position,new Vector3((float)gridSizeX / 2, (float)gridSizeY / 2, 0));
        //If the position.x of the wallTransform is 0, then it is either an East or West wall, otherwise it's a South or North wall.Each wall scales accordingly
        if (wallTransform.position.x != 0)
            wallTransform.localScale = new Vector3(wallTransform.localScale.x, gridSizeY, 1);
        else
            wallTransform.localScale = new Vector3(wallTransform.localScale.x, gridSizeX, 1);
        
    }

    //Instansiates every cell on the Grid. Depending on the maze generation algorithm selected, 
    //the cell walls will either start breaking as the cells are Instantiated, or after every cell has been Instantiated.
    //The first cell created will be at the bottom-left of the grid, and the rest will be generated left to right, bottom to top
    public void GenerateMaze()
    {
        Vector3 cellPosition = new Vector3((-(float)gridSizeX / 2) + 0.5f, (-(float)gridSizeY / 2) + 0.5f, 0);
        string mazeGenerationAlgorith = MazeProperties.MazeGenerationAlgorithm;
        for(int row=1; row<=gridSizeY; row++)
        {
            for (int column=1; column<=gridSizeX; column++)
            {
                Cell newCell = Instantiate(Cell, cellPosition, transform.rotation).GetComponent<Cell>();
                cellInTheGrid[column - 1, row - 1] = newCell;
                newCell.coordinatesOfCellInArray = new Vector2Int(column - 1, row - 1);
                if (mazeGenerationAlgorith.Equals("Binary Tree"))
                {
                    BinaryTreeGeneration(newCell, row, column);
                }
                else if (mazeGenerationAlgorith.Equals("Sidewinder"))
                {
                    SidewinderGeneration(newCell, row, column);
                }
                cellPosition.x = cellPosition.x + 1;
            }
            cellPosition.y = cellPosition.y + 1;
            //Points to the start of the Row
            cellPosition.x = (-(float)gridSizeX / 2) + 0.5f;
        }
        //If we selected "Recursive Backtracking algorithm, the maze generation begins after all cells are Instantiated
        if (mazeGenerationAlgorith.Equals("Recursive Backtracking"))
        {
            RecursiveBacktrackingGeneration();
        }
    }

    /// <summary>
    /// Informs each cell build the walls that correspond to it
    /// </summary>
    private void BuildMazeWalls()
    {
        foreach (Cell mazeCell in cellInTheGrid)
        {
            mazeCell.BuildWalls();
        }
    }

    //For every Cell, the maze generator will randomly decide if it will destroy the north or east wall of the cell.
    //If the cell is located in the top row of the maze, it will alway break it's east wall
    //If the cell is located in the right column of the maze it will always break the north wall
    public void BinaryTreeGeneration(Cell targetCell, int row, int column)
    {
        //randomWallToBreak : 0 = "North" , 1 = "East"
        int randomWallToBreak = Random.Range(0, 2);
        if (((randomWallToBreak == 0) && (row != gridSizeY)) || (column == gridSizeX))
        {
            targetCell.RemoveWallFromCell("North");
        }
        else if (((randomWallToBreak == 1) && (column != gridSizeX)) || (row == gridSizeY))
        {
            targetCell.RemoveWallFromCell("East");
        }
    }

    
    //For every Cell, the maze generator will randomly decide if it will destroy the north or east wall of the cell
    //When a cell destroys a wall, it adds that cell on a cell list. 
    //When a cell is about to destroy a north wall, a random cell from the list is picked and that cell destroys its north wall instead. Then the list is emptied.
    public void SidewinderGeneration(Cell targetCell, int row, int column)
    {
        //randomWallToBreak : 0 = "North" , 1 = "East"
        int randomWallToBreak = Random.Range(0, 2);
        if (((randomWallToBreak == 0) && (row != gridSizeY)) || (column == gridSizeX))
        {
            sidewinderCellSetList.Add(targetCell);
            int randomCell = Random.Range(0, sidewinderCellSetList.Count);
            sidewinderCellSetList[randomCell].RemoveWallFromCell("North");
            sidewinderCellSetList.Clear();
        }
        else if (((randomWallToBreak == 1) && (column != gridSizeX)) || (row == gridSizeY))
        {
            targetCell.RemoveWallFromCell("East");
            sidewinderCellSetList.Add(targetCell);
        }
    }
    
    //The maze generator selects a starting cell at random. Then it looks at its neighboring cells in random order, and sees if any of them have not already been selected
    //If it finds a new suitable cell, it breaks the wall between the two cells and repeats the process.
    //If it can't find any suitable cells, then it returns to the previous selected cell and tries its remaining neigbor cells.
    //When every cell has been selected, then the maze is finished.
    public void RecursiveBacktrackingGeneration()
    {
        //A stack that holds cells that have been selected. Used to go back to previous cells if we reach a dead end
        Stack<Cell> selectedCellsStack = new Stack<Cell>();
        //The position of the selected cell in the wholeMaze array
        Vector2Int selectedCellCoordinates = new Vector2Int(Random.Range(0, gridSizeX), Random.Range(0, gridSizeY));
        //Counts how many cells have been selected
        int selectedCellsCount = 0;
        //The cell that we have selected at the moment
        Cell currentCell = null;
        //Shows if we have a suitable Cell to move to. Starts as true because any starting cell is viable
        bool foundSuitableNeighborCell = true;
        //Keeps count of on how many directions we have tried to find a suitable next cell, without success. 
        //If it reaches 4 it means we can't finda suitable neibouring cell, and we must return to a previous cell
        int noOfTriedDirections =0;
        List<string> directionsList = new List<string>() { "North", "East", "South", "West" };
        do
        {
            directionsList.Shuffle();
            //The loop will check randomly every direction around the selected cell, until we find a suitable cell to move to. 
            //If it finds a cell, it destroys the wall between the two cells and saves the new cell's coordinates.  
            //It keeps count of how many directions we have tried, so we know if we tried every direction without finding a suitable cell
            foreach (string direction in directionsList)
            {
                if((direction.Equals("North"))&&(!foundSuitableNeighborCell))
                {
                    noOfTriedDirections++;
                    if ((selectedCellCoordinates.y + 1 < gridSizeY) && (cellInTheGrid[selectedCellCoordinates.x, selectedCellCoordinates.y + 1].selectedInRecursiveBacktracking == false))
                    {
                        currentCell.RemoveWallFromCell("North");
                        foundSuitableNeighborCell = true;
                        selectedCellCoordinates.y++;
                    }
                }
                if((direction.Equals("East")) && (!foundSuitableNeighborCell))
                {
                    noOfTriedDirections++;
                    if ((selectedCellCoordinates.x + 1 < gridSizeX) && (cellInTheGrid[selectedCellCoordinates.x + 1, selectedCellCoordinates.y].selectedInRecursiveBacktracking == false))
                    {
                        currentCell.RemoveWallFromCell("East");
                        foundSuitableNeighborCell = true;
                        selectedCellCoordinates.x++;
                    }
                }
                if((direction.Equals("South")) && (!foundSuitableNeighborCell))
                {
                    noOfTriedDirections++;
                    if ((selectedCellCoordinates.y - 1 > -1) && (cellInTheGrid[selectedCellCoordinates.x, selectedCellCoordinates.y - 1].selectedInRecursiveBacktracking == false))
                    {
                        cellInTheGrid[selectedCellCoordinates.x, selectedCellCoordinates.y - 1].RemoveWallFromCell("North");
                        foundSuitableNeighborCell = true;
                        selectedCellCoordinates.y--;
                    }
                }
                if((direction.Equals("West")) && (!foundSuitableNeighborCell))
                {
                    noOfTriedDirections++;
                    if ((selectedCellCoordinates.x - 1 > -1) && (cellInTheGrid[selectedCellCoordinates.x - 1, selectedCellCoordinates.y].selectedInRecursiveBacktracking == false))
                    {
                        cellInTheGrid[selectedCellCoordinates.x - 1, selectedCellCoordinates.y].RemoveWallFromCell("East");
                        foundSuitableNeighborCell = true;
                        selectedCellCoordinates.x--;
                    }
                        
                }
            }
            //When a suitable Neighbor Cell has been found, it becomes the current cell
            if (foundSuitableNeighborCell)
            {
                currentCell = cellInTheGrid[selectedCellCoordinates.x, selectedCellCoordinates.y];
                currentCell.selectedInRecursiveBacktracking = true;
                selectedCellsCount++;
                selectedCellsStack.Push(currentCell);
                noOfTriedDirections = 0;
                foundSuitableNeighborCell = false;
            }
            //If we couldn't find a suitable cell in any direction, then we Pop back the previous selected cell from the selectedCellsStack
            if (noOfTriedDirections>=3)
            {
                noOfTriedDirections = 0;
                if (selectedCellsStack.Count > 0)
                {
                    currentCell=selectedCellsStack.Pop();
                }
                selectedCellCoordinates = currentCell.coordinatesOfCellInArray;
            }
        //If all Cells on the grid have been selected, the maze is complete
        } while (selectedCellsCount!=gridSizeX*gridSizeY);
    }





}
