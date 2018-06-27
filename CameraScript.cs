using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//When the maze scene starts, the camera will start zooming out until the both "bottomLeft" and "topRight" objects are visible
//This makes sure that the maze will always be visible, with the minimum amount of background
public class CameraScript : MonoBehaviour {

    public GameObject bottomLeft;
    public GameObject topRight;
    private float zoomingSpeed;
    private Camera thisCam;
    private Renderer bottomLeftRenderer;
    private Renderer topRightRenderer;

	void Start ()
    {
        thisCam=GetComponent<Camera>();
        bottomLeftRenderer = bottomLeft.GetComponent<Renderer>();
        topRightRenderer = topRight.GetComponent<Renderer>();
        //Depending on the size of the maze, the zooming speed should be higher or lower
        zoomingSpeed = ((float)MazeProperties.MazeWidth + (float)MazeProperties.MazeHeight) / 100f;
        //This position represents the bottomLeft of the Maze. It has some offset to make sure the Button Canvas doesn't block the maze at all
        Vector3 bottomLeftPosition = new Vector3((-(float)MazeProperties.MazeWidth / 2) - (0.1f* MazeProperties.MazeWidth/10), -((float)MazeProperties.MazeHeight / 2) - (1.3f * MazeProperties.MazeHeight / 10), -0.3f);
        //This position represents the top right of the Maze. Since there are not buttons on the top, the offset is smaller
        Vector3 topRightPosition = Vector3.Scale(bottomLeftPosition, new Vector3(-1, -0.8f, 0.8f));
        bottomLeft.transform.position = bottomLeftPosition;
        topRight.transform.position = topRightPosition;
    }
	
	void Update ()
    {
        if((!bottomLeftRenderer.isVisible)&&(!topRightRenderer.isVisible))
        {
            thisCam.orthographicSize += zoomingSpeed;
        }
        //The camera changes its position depending on the zoom level, to keep the Maze centered regardless of the Button canvas
        transform.position = new Vector3(transform.position.x, -thisCam.orthographicSize / 10, transform.position.z);
	}
}
