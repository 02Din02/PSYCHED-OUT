using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    // Movement stuff for changing values :D
    public float moveSpeed; 
    public float jumpForce;

    // References to components :)
    private Rigidbody2D playerRb;
    private Animator playerAnim;

    // Bools :>
    public bool isGrounded = false;
    public bool isJumping = false;

    // For Input
    private Vector2 horizontalInput;
    

    void Start()
    {
        // Setting up the components!!
        playerRb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
    }

   public void OnMove(InputAction.CallbackContext context)
   {
        horizontalInput = context.ReadValue<Vector2>();
        Debug.Log(horizontalInput);
   }

   public void OnJump(InputAction.CallbackContext context)
   {
     Debug.Log("Wowie I just pressed SpaceBar!!!");
   }



    //void Update()
   // {
        
   // }
}
