using UnityEngine;
using System.Collections;
 
public class CharacterController2D : MonoBehaviour
{
 
    float horizontal;
    float vertical;
    float moveLimiter = 0.7f;
    public float runSpeed = 20.0f;
    private Rigidbody2D player;       //Store a reference to the Rigidbody2D component required to use 2D Physics.


    void Start()
    {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        player = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Gives a value between -1 and 1
        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down
    }
 
    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0) // Check for diagonal movement
    {
        // limit movement speed diagonally, so you move at 70% speed
        horizontal *= moveLimiter;
        vertical *= moveLimiter;
    } 

        player.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
    }
}



