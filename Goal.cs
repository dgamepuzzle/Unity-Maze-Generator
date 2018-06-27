using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//When the player touches the Goal's trigger collider, the solveMazeButton is informed, and both the goal and player are destroyed
public class Goal : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            transform.parent.GetComponent<SolveMazeButton>().PlayerReachedTheGoal();
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
