using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//When the button is pressed, it collects all the input data from the Main Menu, and saves them using the MazeProperties static class
public class GenerateMazeButton : MonoBehaviour
{
    public InputField mazeHeightField;
    public InputField mazeWidthField;
    public Dropdown algorithSelectionDropdown;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick()
    {
        //If either textfield is empty, the button does nothing
        if((mazeHeightField.text.Equals(""))||(mazeWidthField.text.Equals("")))
        {
            return;
        }
        MazeProperties.MazeHeight = System.Int32.Parse(mazeHeightField.text);
        MazeProperties.MazeWidth = System.Int32.Parse(mazeWidthField.text);
        string selectedAlgorithm=null;
        switch (algorithSelectionDropdown.value)
        {
            case 0:
                selectedAlgorithm ="Binary Tree";
                break;
            case 1:
                selectedAlgorithm ="Sidewinder";
                break;
            case 2:
                selectedAlgorithm = "Recursive Backtracking";
                break;
        }
        MazeProperties.MazeGenerationAlgorithm=selectedAlgorithm;
        if ((MazeProperties.MazeHeight>2)&&(MazeProperties.MazeWidth>2))
            SceneManager.LoadScene(1);

    }

}
