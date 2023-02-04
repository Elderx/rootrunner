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
    public Camera cam; 

    private Rigidbody2D player;       //Store a reference to the Rigidbody2D component required to use 2D Physics.
    private Transform playerTransform;

    private List<Vector3> visitedPoints = new List<Vector3>();
    private float _verticalInput = 0;
    private float _horizontalInput = 0;

    public float playerMinWidth = 0.1f;
    public float playerWidth = 0.1f;
    private float sizeChange = 0.01f;

    private float zoomMin = 3f;

    void Start()
    {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        player = GetComponent<Rigidbody2D>();

        player.transform.position = startingPosition.transform.position;

        visitedPoints.Add(startingPosition.transform.position);

        lineRenderer = GetComponent<LineRenderer>();

        playerTransform = player.GetComponent<Transform>();
        playerTransform.localScale = new Vector3(playerWidth, playerWidth, 0.5f);

        lineRenderer.startWidth = playerWidth;

        cam.orthographicSize = GetCameraZoom();
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

      lineRenderer.endWidth = playerWidth;

      if (visitedPoints.Count > 1) {
        lineRenderer.SetPosition(visitedPoints.Count - 1, player.position);
      }
    }

    void UpdateCamera() {
      cam.transform.position = new Vector3(player.position.x, player.position.y, cam.transform.position.z);
    }

    float GetCameraZoom() {
      return playerWidth * 10 + zoomMin;
    }
 
    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
      MovePlayer();
      RotatePlayer();
      UpdateCamera();

      PlayerSizeChange(-sizeChange * 0.01f);

      Vector2 lastPosition = visitedPoints[visitedPoints.Count - 1];

      if (Vector2.Distance(lastPosition, player.position) > visitTreshHold) {
        float x = player.position.x;
        float y = player.position.y;

        visitedPoints.Add(player.position);
      }
    }

    void PlayerSizeChange(float change) {
      float newSize = player.transform.localScale.x + change;

      if (newSize > playerMinWidth) {
        playerWidth = newSize;
        playerTransform.localScale = new Vector3(playerWidth, playerWidth, 0);

        cam.orthographicSize = GetCameraZoom();
      }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Water")
        {
          PlayerSizeChange(sizeChange);
        }
    }
}



