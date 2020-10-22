using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBallController : MonoBehaviour
{
    [SerializeField] private float thrust = 140f;
    [SerializeField] private Rigidbody ballRigidbody;
    [SerializeField] private float wallDistance = 5f;
    [SerializeField] private float minCamDistance = 4.5f;
    [SerializeField] private int ballSpeed = 5;

    private Vector2 lastMousePos;

    // Update is called once per frame
    void Update()
    {
        // Move the player ball according to the mouse when pressed
        Vector2 deltaMousePos = Vector2.zero;

        if (Input.GetMouseButton(0))
        {
            // Signals the Game has started and only runs it once if the game has already started
            if (!GameManager.singleton.GameStarted)
                GameManager.singleton.StartGame();

            Vector2 currentMousePos = Input.mousePosition;

            if (lastMousePos == Vector2.zero)
                lastMousePos = currentMousePos;

            deltaMousePos = currentMousePos - lastMousePos;

            lastMousePos = currentMousePos;

            Vector3 force = new Vector3(deltaMousePos.x, 0, deltaMousePos.y) * thrust;
            ballRigidbody.AddForce(force);
        }
        else
        {
            lastMousePos = Vector2.zero;
        }
    }

    // Late Update used mainly for Camera Calculations and Calculations that need to occur after movement has occured
    // Occurs after physics is applied 
    private void LateUpdate()
    {
        Vector3 ballOldPos = transform.position;

        // To make the ball not fall from the floor
        if (transform.position.x < -wallDistance)
            ballOldPos.x = -wallDistance;

        else if (transform.position.x > wallDistance)
            ballOldPos.x = wallDistance;


        // To make the ball always be in front of the camera and never behind it
        float ballLowestPostion = Camera.main.transform.position.z + minCamDistance;

        if (transform.position.z < ballLowestPostion)
            ballOldPos.z = ballLowestPostion;

        transform.position = ballOldPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Memory Improvements
        if (GameManager.singleton.GameEnded)
            return;

        // Signals the Game has ended
        if (collision.gameObject.tag == "Death")
            GameManager.singleton.EndGame(false);

    }

    private void FixedUpdate()
    {
        // Memory Improvements
        if (GameManager.singleton.GameEnded)
            return;

        if (GameManager.singleton.GameStarted)
            ballRigidbody.MovePosition(transform.position + Vector3.forward * ballSpeed * Time.fixedDeltaTime);
    }

}
