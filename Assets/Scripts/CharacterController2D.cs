using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class CharacterController2D : MonoBehaviour
{
 
    float horizontal;
    float vertical;

    public float rotationSpeed = 5.0f;

    public float runSpeed = 5.0f;
    private float minX = -8.5f;
    private float maxX = 8.5f;
    public GameObject startingPosition;
    public float visitTreshHold = 0.1f;
    public LineRenderer lineRenderer;
    public Camera cam; 
    public AudioSource audioSource;
    public AudioClip drinkSound;
    public AudioClip crashSound;
    public Light2D worldLight;

    // Rootlight pulse control
    public Light2D rootlight;
    public float elapsedTime;
    
    public float maxBrightness = 5.0f;
    public float das = 1.0f;
    private int direction = -1;

    private Rigidbody2D player;       //Store a reference to the Rigidbody2D component required to use 2D Physics.
    private Transform playerTransform;

    private List<Vector3> visitedPoints = new List<Vector3>();
    // private float _verticalInput = 0;
    private float _horizontalInput = 0;

    public float playerWidth = 0.1f;
    private float sizeChange = 0.01f;

    private float zoomMin = 3f;

    public TMP_Text waterText;
    public TMP_Text depthText;
    public TMP_Text highscoreText;

    void Start()
    {

        Debug.Log("Character controller start");
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        player = GetComponent<Rigidbody2D>();

        player.transform.position = startingPosition.transform.position;

        visitedPoints.Add(startingPosition.transform.position);

        lineRenderer = GetComponent<LineRenderer>();

        playerTransform = player.GetComponent<Transform>();
        playerTransform.localScale = new Vector3(playerWidth, playerWidth, 0.5f);

        lineRenderer.startWidth = playerWidth;


        if (PlayerPrefs.HasKey("highscore") == false) {
            PlayerPrefs.SetInt("highscore", 0);
            PlayerPrefs.Save();
        }
        else {
            highscoreText.text = "Highscore: " + PlayerPrefs.GetInt("highscore").ToString();
        }

        Vector2 topLeft = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, cam.nearClipPlane));
        Vector2 topRight = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane));

        minX = topLeft.x;
        maxX = topRight.x;

        Debug.Log("Before in start: " + rootlight.transform.rotation.eulerAngles);

        rootlight.transform.rotation.eulerAngles.Set(0, 0, 180f);

        Debug.Log("After in start: " + rootlight.transform.rotation.eulerAngles);

        
    }

    void Update()
    {
        GetPlayerInput();

        MovePlayer();

        renderLine();

        // ENABLE_KILL_MODE();
    }

    private void GetPlayerInput() {
      _horizontalInput = Input.GetAxisRaw("Horizontal"); // -1 is left
    }

    private void MovePlayer()
    {
      player.velocity = (transform.up * runSpeed) * -1;

      if (player.velocity.y > 0) {
        player.velocity = new Vector2(player.velocity.x, 0);
      }

      if(player.position.x >= maxX) {
        player.position = new Vector2(maxX, player.position.y);
      } else if (player.position.x <= minX) {
        player.position = new Vector2(minX, player.position.y);
      }

      UpdateRootLight();
    }

    private void RotatePlayer()
    {
        float rotation = _horizontalInput * rotationSpeed;
        Vector3 newRotate = Vector3.forward * -rotation;

        if(this.player.rotation >= 60 && rotation < 0) {
          this.player.rotation = 60;
        } else if (this.player.rotation <= -60 && rotation > 0) {
          this.player.rotation = -60;
        } else {
          transform.Rotate(newRotate);
        }
    }


    void renderLine () {
      lineRenderer.positionCount = visitedPoints.Count;

      lineRenderer.SetPositions(visitedPoints.ToArray());

      lineRenderer.endWidth = playerWidth;

      if (visitedPoints.Count > 1) {
        lineRenderer.SetPosition(visitedPoints.Count - 1, player.position);
      }
    }

    void UpdateCamera() {
      cam.transform.position = new Vector3(0, player.position.y, cam.transform.position.z);
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

        if (visitedPoints.Count > 1000) {
          visitedPoints.RemoveAt(0);
        }
      }

      waterText.text = "Water: " + (Mathf.Round(playerWidth * 100));
      depthText.text = "Depth: " + (Mathf.Abs(Mathf.Round(player.position.y)));


      if (playerWidth <= 0) {
        GameOver();
      }
    }

    void GameOver() {
      int currentRecord = PlayerPrefs.GetInt("highscore");

      int score = (int)Mathf.Abs(Mathf.Round(player.position.y));

      if (currentRecord < score) { 
        NavigationManager.newRecord = true;

        PlayerPrefs.SetInt("highscore", score);  
        PlayerPrefs.Save();
      } else {
        NavigationManager.newRecord = false;
      }

      NavigationManager.score = score;

      SceneManager.LoadScene("MainScene");
      NavigationManager.GameOver = true;
    }

    void UpdateRootLight () {
      float force = 1 - worldLight.intensity;

      rootlight.intensity = Mathf.Min(2, force * playerTransform.localScale.x * 25);
      rootlight.transform.position = player.position;
      rootlight.transform.rotation = player.transform.rotation;

      rootlight.transform.Rotate(new Vector3(0, 0, 180));

      //Debug.Log("Angle before: " + angle);


      //Debug.Log("Angle after: " + angle);
    }

    void PlayerSizeChange(float change) {
      float newSize = player.transform.localScale.x + change;

      playerWidth = newSize;
      playerTransform.localScale = new Vector3(playerWidth, playerWidth, 0);

      //UpdateRootLight();
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.gameObject.tag == "Water")
        {
          audioSource.PlayOneShot(drinkSound);
          PlayerSizeChange(sizeChange);
          Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "Obstacle") {
          audioSource.PlayOneShot(crashSound);
          PlayerSizeChange(-sizeChange);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Obstacle")
        {
          GameOver();
        }
    }

    void ENABLE_KILL_MODE() 
    {
    elapsedTime = Time.realtimeSinceStartup;
      if (elapsedTime > 5) {
        float addPart = direction < 0 ? -1 * (das * Time.deltaTime - 5) : das * Time.deltaTime - 5;
        float intensity = rootlight.GetComponent<Light2D>().intensity + addPart;

        rootlight.GetComponent<Light2D>().intensity = intensity;

        if (intensity <= 0 || intensity >= maxBrightness) direction *= -1;
      }
    }
}



