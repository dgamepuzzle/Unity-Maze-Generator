using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The maze generator selects a starting cell at random. Then it looks at its neighboring cells in random order, and sees if any of them have not already been selected
//If it finds a new suitable cell, it breaks the wall between the two cells and repeats the process.
//If it can't find any suitable cells, then it returns to the previous selected cell and tries its remaining neigbor cells.
//When every cell has been selected, then the maze is finished.
public class RecursiveBacktrackingAlgorithm : MazeGeneration
{

    private enum Directions {North,East, South,West};
    private Vector2Int selectedCellCoordinates;
    protected override IEnumerator CalculateMazeWalls()
    {
        //A stack that holds cells that have been selected. Used to go back to previous cells if we reach a dead end
        Stack<Cell> selectedCellsStack = new Stack<Cell>();
        //The position of the selected cell in the wholeMaze array
        selectedCellCoordinates = new Vector2Int(Random.Range(0, (int)gridSizeX), Random.Range(0, (int)gridSizeY));
        //Counts how many cells have been selected
        int selectedCellsCount = 0;
        //The cell that we have selected at the moment
        Cell currentCell = null;
        //Shows if we have a suitable Cell to move to. Starts as true because any starting cell is viable
        bool foundSuitableNeighborCell = true;
        //Keeps count of on how many directions we have tried to find a suitable next cell, without success. 
        //If it reaches 4 it means we can't finda suitable neibouring cell, and we must return to a previous cell
        int noOfTriedDirections = 0;
        List<Directions> directionsList = new List<Directions>() { Directions.North, Directions.East, Directions.South, Directions.West };
        do
        {
            directionsList.Shuffle();
            //The loop will check randomly every direction around the selected cell, until we find a suitable cell to move to. 
            //If it finds a cell, it destroys the wall between the two cells and saves the new cell's coordinates.  
            //It keeps count of how many directions we have tried, so we know if we tried every direction without finding a suitable cell
            foreach (Directions direction in directionsList)
            {
                noOfTriedDirections++;
                if (( (selectedCellCoordinates.y + 1 < gridSizeY) && (direction == Directions.North) )|| 
                    ( (selectedCellCoordinates.x + 1 < gridSizeX) && (direction == Directions.East) ) || 
                    ( (selectedCellCoordinates.y - 1 > -1) && (direction == Directions.South) ) || 
                    ((selectedCellCoordinates.x - 1 > -1) && (direction == Directions.West) ))
                {
                    if ((direction == Directions.North) && (!foundSuitableNeighborCell))
                    {
                        foundSuitableNeighborCell = CheckForEligibleCell(currentCell, (cellInTheGrid[selectedCellCoordinates.x, selectedCellCoordinates.y + 1]));
                    }
                    else if ((direction == Directions.East) && (!foundSuitableNeighborCell))
                    {
                        foundSuitableNeighborCell = CheckForEligibleCell(currentCell, (cellInTheGrid[selectedCellCoordinates.x + 1, selectedCellCoordinates.y]));
                    }
                    else if ((direction == Directions.South) && (!foundSuitableNeighborCell))
                    {
                        foundSuitableNeighborCell = CheckForEligibleCell(currentCell, (cellInTheGrid[selectedCellCoordinates.x, selectedCellCoordinates.y - 1]));
                    }
                    else if ((direction == Directions.West) && (!foundSuitableNeighborCell))
                    {
                        foundSuitableNeighborCell = CheckForEligibleCell(currentCell, (cellInTheGrid[selectedCellCoordinates.x - 1, selectedCellCoordinates.y]));
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
            else if (noOfTriedDirections >= 3)
            {
                noOfTriedDirections = 0;
                if (selectedCellsStack.Count > 0)
                {
                    currentCell = selectedCellsStack.Pop();
                }
                selectedCellCoordinates = currentCell.coordinatesOfCellInGridArray;
            }

            //yields after every 200 cells have been checked, in order for game to not freeze for too long in big grids
            if(selectedCellsCount%200==0)
            {
                yield return null;
            }

            //If all Cells on the grid have been selected, the maze is complete
        } while (selectedCellsCount != gridSizeX * gridSizeY);
        BuildMazeWalls();
    }

    private bool CheckForEligibleCell(Cell currentCell, Cell cellToCheck)
    {

        Vector2Int cellPositionDifference = new Vector2Int(cellToCheck.coordinatesOfCellInGridArray.x - currentCell.coordinatesOfCellInGridArray.x,
                                                            cellToCheck.coordinatesOfCellInGridArray.y - currentCell.coordinatesOfCellInGridArray.y);

        if (cellToCheck.selectedInRecursiveBacktracking == false)
        {
            if (cellPositionDifference.x > 0)
            {
                currentCell.RemoveWallFromCell("East");
            }
            else if (cellPositionDifference.x < 0)
            {
                cellToCheck.RemoveWallFromCell("East");
            }
            else if (cellPositionDifference.y > 0)
            {
                currentCell.RemoveWallFromCell("North");
            }
            else if (cellPositionDifference.y < 0)
            {
                cellToCheck.RemoveWallFromCell("North");
                
            }
            selectedCellCoordinates += cellPositionDifference;
            return true;
        }
        else
        {
            return false;
        }
    }
}
