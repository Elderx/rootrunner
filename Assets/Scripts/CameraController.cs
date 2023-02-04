using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour 
{
    public Transform background;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
      float cameraHeight = Mathf.Ceil(Camera.main.orthographicSize * 2);
      float cameraWidth = Mathf.Ceil(cameraHeight * Camera.main.aspect);

      background.GetComponent<SpriteRenderer>().size = new Vector2(cameraWidth, cameraHeight);
    }
}
