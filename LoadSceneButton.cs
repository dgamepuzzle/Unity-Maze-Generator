using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//When the Button is clicked, the selected scene will be loaded
[RequireComponent(typeof(Button))]
public class LoadSceneButton : MonoBehaviour
{
    [SerializeField]
    private int sceneToLoad;

	private void Start ()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
	}

    private void TaskOnClick()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
