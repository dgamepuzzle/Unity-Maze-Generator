using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public float speed;
    private Rigidbody playerRigidbody;

	void Start ()
    {
        playerRigidbody=GetComponent<Rigidbody>();
		
	}
	
	void Update ()
    {
        Vector3 newPosition = transform.position;
        if(Input.GetKey(KeyCode.W))
        {
            newPosition.y += speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            newPosition.x -= speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            newPosition.y -= speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            newPosition.x += speed;
        }

        #if UNITY_ANDROID
        Vector3 mobileNewPosition = transform.position;
        if (Input.touchCount>0)
        {
            mobileNewPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            if (mobileNewPosition.x>transform.position.x)
            {
                newPosition.x += speed;
            }
            if (mobileNewPosition.x < transform.position.x)
            {
                newPosition.x -= speed;
            }
            if (mobileNewPosition.y > transform.position.y)
            {
                newPosition.y += speed;
            }
            if (mobileNewPosition.y < transform.position.y)
            {
                newPosition.y -= speed;
            }
        }
        #endif

        playerRigidbody.MovePosition(newPosition);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            transform.parent.GetComponent<SolveMazeButton>().PlayerReachedTheGoal();
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
