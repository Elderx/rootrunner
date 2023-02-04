using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class CharacterController2D : MonoBehaviour
{
 
    float horizontal;
    float vertical;
    float moveLimiter = 0.7f;

    public float rotationSpeed = 5.0f;

    public float runSpeed = 5.0f;
    public GameObject startingPosition;
    public float visitTreshHold = 0.1f;
    public LineRenderer lineRenderer;

    private Rigidbody2D player;       //Store a reference to the Rigidbody2D component required to use 2D Physics.
    private List<Vector3> visitedPoints = new List<Vector3>();
    private float _verticalInput = 0;
    private float _horizontalInput = 0;


    void Start()
    {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        player = GetComponent<Rigidbody2D>();

        player.transform.position = startingPosition.transform.position;

        visitedPoints.Add(startingPosition.transform.position);

        // float playerRadius = player.GetComponent<CircleCollider2D>().bounds;
        // lineRenderer.SetWidth(playerRadius, playerRadius);
    }

    void Update()
    {
        GetPlayerInput();

        renderLine();
    }

    private void GetPlayerInput() {
      _horizontalInput = Input.GetAxisRaw("Horizontal"); // -1 is left
      _verticalInput = Input.GetAxisRaw("Vertical"); // -1 is down
    }

    private void MovePlayer()
    {
        player.velocity = transform.right * Mathf.Clamp01(_verticalInput) * runSpeed;
    }

    private void RotatePlayer()
    {
        float rotation = _horizontalInput * rotationSpeed;
        transform.Rotate(Vector3.forward * -rotation);
    }


    void renderLine () {
      lineRenderer.SetVertexCount(visitedPoints.Count);

      lineRenderer.SetPositions(visitedPoints.ToArray());

      if (visitedPoints.Count > 1) {
        lineRenderer.SetPosition(visitedPoints.Count - 1, player.position);
      }
    }
 
    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
    /*
      if (horizontal != 0 && vertical != 0) // Check for diagonal movement
      {
          // limit movement speed diagonally, so you move at 70% speed
          horizontal *= moveLimiter;
          vertical *= moveLimiter;
      } 

      player.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);

      */

      Vector2 lastPosition = visitedPoints[visitedPoints.Count - 1];

      if (Vector2.Distance(lastPosition, player.position) > visitTreshHold) {
        visitedPoints.Add(player.transform.position);
      }

    }
}



