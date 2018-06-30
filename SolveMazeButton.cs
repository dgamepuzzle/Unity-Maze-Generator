using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//When the button is clicked, it creates a Player and a Goal gameobject.
//When the player touches the Goal's trigger collider, this button is informed and enables the congratulations text
//The text dissapears after 3 seconds, or when the button is clicked again
[RequireComponent(typeof(Button))]
public class SolveMazeButton : MonoBehaviour
{
    private bool gameIsOngoing=false;
    public Text congratulationsText;
    public GameObject player;
    public GameObject goal;
    
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        if (!gameIsOngoing)
        {
            StopCoroutine(DisableTextInSeconds(5));
            congratulationsText.enabled = false;
            gameIsOngoing = true;
            Vector3 playerPosition = new Vector3(((float)MazeProperties.MazeWidth / 2) - 0.5f, ((float)MazeProperties.MazeHeight / 2) - 0.5f, -0.3f);
            //The player will be a child of the button, in order to inform it when he reached the object
            Instantiate(player, playerPosition, transform.rotation).transform.parent=transform;
            Instantiate(goal, Vector3.Scale(playerPosition, new Vector3(-1, -1, 0.8f)), transform.rotation);
        }
    }

    public void PlayerReachedTheGoal()
    {
        congratulationsText.enabled = true;
        gameIsOngoing = false;
        StartCoroutine(DisableTextInSeconds(3));
    }

    IEnumerator DisableTextInSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        congratulationsText.enabled = false;
    }


}
