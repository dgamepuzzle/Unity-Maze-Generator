using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//When the Button is clicked, the selected scene will be loaded
public class LoadSceneButton : MonoBehaviour
{
    public int sceneToLoad;

	void Start ()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
	}

    void TaskOnClick()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
