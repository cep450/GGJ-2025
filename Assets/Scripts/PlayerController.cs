using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Fields
    [Header("Movement")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float movementSpeed;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 directionToMove;
    [SerializeField] private float groundDrag;

    [Header("Grounded Logic")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;

    [Header("Jumping Logic")]
    [SerializeField] private float jumpForce; //haha thats a reference
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    private bool canJump = true;
    
    #endregion Fields
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Get Inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");


        //Check if Grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight + 0.2f, groundLayer);
        CheckToJump();
        //Actually Move Player
        MovePlayer();
        //Logic based on grounded or airborn
        if(isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void MovePlayer()
    {
        directionToMove = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(isGrounded)
        {
            rb.AddForce(directionToMove.normalized * Time.deltaTime * movementSpeed, ForceMode.Force);
        }
        else
        {
            rb.AddForce(directionToMove.normalized * Time.deltaTime * movementSpeed * airMultiplier, ForceMode.Force);
        }

        Vector3 gravityLessVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (gravityLessVelocity.magnitude > movementSpeed)
        {
            Vector3 newVelocity = rb.velocity.normalized * movementSpeed;
            rb.velocity = new Vector3(newVelocity.x, rb.velocity.y, newVelocity.z);
        }
    }

    private void CheckToJump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            canJump = false;
            Jump();
            Invoke("ResetJump", jumpCooldown);
        }
    }

    private void Jump()
    {
        print("Jump");
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce ,ForceMode.Impulse);
    }

    private void ResetJump()
    {
        canJump = true;
    }
}
