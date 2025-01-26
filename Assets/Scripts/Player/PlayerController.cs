using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMODUnity;

public class PlayerController : MonoBehaviour
{
    #region Fields
    private enum MovementState
    {
        Walking,
        Running,
        Crouching,
        Sliding,
        Airborn
    }
    [Header("Movement")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float groundDrag;
	[SerializeField] private float airDrag;

    [Header("Movement Speeds")]
    private float movementSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float crouchSpeed;

    [Header("Inputs")]
    private float horizontalInput;
    private float verticalInput;
    private Vector3 directionToMove;

    [Header("Grounded Logic")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;
    private bool previousGroundState;

    [Header("Jumping Logic")]
    [SerializeField] private float jumpForce; //haha thats a reference
	[SerializeField] private float jumpForceForward;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
	[SerializeField] private int maxJumps = 2;
	[SerializeField] private float jumpBufferTime = 0.25f;
    private bool canJump = true;
	private int jumpCount = 0;
	private float jumpCountdown = 0f;
    private float jumpBuffer = 0.0f;

	[Header("Ground Slam Logic")]
	[SerializeField] private float slamForce;
	private bool slamming = false;
	private GameObject slamLines;

    [Header("Sprinting Logic")]
    public bool isSprinting;
    private float sprintWindow = 0.5f;
    private float sprintWindowCounter;
    private int sprintButtonPresses = 0;

    [Header("Crouching Logic")]
    [SerializeField] private float crouchHeight;
    private float normalHeight;

    [Header("Slope Logic")]
    [SerializeField] private float maxSlopeAngle;
    RaycastHit raycastSlopeHit;

    [Header("AudioStuff")]
    [SerializeField] private StudioEventEmitter walkingEmmitter;
    [SerializeField] private StudioEventEmitter runningEmmitter;
    [SerializeField] private StudioEventEmitter jumpEmmitter;
    [SerializeField] private StudioEventEmitter landingEmmitter;
    private StudioEventEmitter currentEmmitter;
    /*
    [SerializeField] private AudioClip runningSFX;
    [SerializeField] private AudioClip[] walkingSFXs;
    [SerializeField] private AudioSource audioSource;
    */
    private MovementState lastMovementState;
    private MovementState movementState;

    [Header("Debug")]
    [SerializeField] private bool debug;
    [SerializeField] TextMeshProUGUI speedText;
    #endregion Fields
    void Start()
    {
        sprintWindowCounter = sprintWindow;
        normalHeight = transform.localScale.y;

		slamLines = GameObject.FindGameObjectWithTag("SlamLines");
		slamLines.SetActive(false);
    }

    void Update()
    {


        //Check if Grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, groundLayer);

        if(isGrounded && !previousGroundState)
        {
            landingEmmitter.Play();
        }

		//Update jump, ground slam state
		JumpUpdate();
		SlamUpdate();

        Inputs();

        //Evaluate State of player 
        StateHandler();

        //Actually Move Player
        MovePlayer();

        //Drag Logic based on grounded or airborn
        if(isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }

        if(debug)
        {
            speedText.text = "State is " + movementState.ToString();
        }
        lastMovementState = movementState;
        previousGroundState = isGrounded;
    }

    private void StateHandler()
    {
        if(isGrounded)
        {
            if (Input.GetKey(KeyCode.LeftShift) && movementState != MovementState.Sliding)
            {
                movementState = MovementState.Crouching;
                movementSpeed = crouchSpeed;
            }
            else if (isSprinting)
            {
                movementState = MovementState.Running;
                movementSpeed = sprintSpeed;
            }
            else
            {
                movementState = MovementState.Walking;
                movementSpeed = walkSpeed;
            }
        }

        else
        {
            movementState = MovementState.Airborn;
        }


    }

    private void MovePlayer()
    {
        directionToMove = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //On Slope
        if(OnSlope())
        {
            rb.AddForce(GetSlopeMovementAngle() * Time.deltaTime * movementSpeed , ForceMode.Force);
        }

        else if(isGrounded)
        {
			// walking
            rb.AddForce(directionToMove.normalized * Time.deltaTime * movementSpeed, ForceMode.Force);
        }
        else
        {
			// air control- ignore forward input, but listen to sideways and backwards
			directionToMove = (orientation.forward * Mathf.Min(verticalInput, 0)) + orientation.right * horizontalInput;
            rb.AddForce(directionToMove.normalized * Time.deltaTime * movementSpeed * airMultiplier, ForceMode.Force);
        }

        //FMOD moving
        if (movementState != lastMovementState && (verticalInput !=0 || horizontalInput != 0))
        {
            if(currentEmmitter != null)
            {
                currentEmmitter.Stop();
            }
            switch (movementState)
            {
                case (MovementState.Walking):
                {
                    walkingEmmitter.Play();
                    currentEmmitter = walkingEmmitter;
                }
                break;
                case (MovementState.Running):
                    {
                        runningEmmitter.Play();
                        currentEmmitter = runningEmmitter;
                    }
                    break;
                case (MovementState.Crouching):
                    {
                        walkingEmmitter.Play();
                        currentEmmitter = walkingEmmitter;
                    }
                    break;
            }
        }
        else if(verticalInput == 0 && horizontalInput == 0)
        {
            if(currentEmmitter != null)
            {
                currentEmmitter.Stop();
            }
        }

        SpeedControl();

        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        if(OnSlope())
        {
            if(rb.velocity.magnitude > movementSpeed * Time.deltaTime)
            {
                rb.velocity = rb.velocity.normalized * movementSpeed * Time.deltaTime;
            }
        }
        else
        {
            Vector3 gravityLessVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (gravityLessVelocity.magnitude > (movementSpeed * Time.deltaTime))
            {
                Vector3 newVelocity = rb.velocity.normalized * movementSpeed * Time.deltaTime;
                rb.velocity = new Vector3(newVelocity.x, rb.velocity.y, newVelocity.z);
            }
        }

    }

    private void Inputs()
    {
        //Get Inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");


        SprintCheck();

        if (Input.GetKeyDown(KeyCode.Space) && canJump && jumpCount < maxJumps && jumpCountdown <= 0f)
        {
            print(jumpCount);
			// double jump 
			jumpCount++;
            Jump();
			jumpCountdown = jumpCooldown;
            jumpBuffer = jumpBufferTime;
        }
        if(isGrounded && Input.GetKeyDown(KeyCode.LeftShift))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);
            //Push player down a bit when they crouch
            rb.AddForce(Vector3.down * 5.0f, ForceMode.Impulse);
        }
        else if(isGrounded && Input.GetKeyUp(KeyCode.LeftShift))
        {
            transform.localScale = new Vector3(transform.localScale.x, normalHeight, transform.localScale.z);
        }
		else if(!isGrounded && !slamming && Input.GetKeyDown(KeyCode.LeftControl)) {

			GroundSlam();

		}
    }

	// for double jumps 
	private void JumpUpdate() {

		jumpCountdown -= Time.deltaTime; 
		if(jumpCountdown <= 0) {
			jumpCountdown = 0; 
		}

		if(isGrounded) {
            if(jumpBuffer <= 0)
            {
                jumpCount = 0;
            }
			else
            {
                jumpBuffer -= Time.deltaTime;
            }
		}
	}

    private void Jump()
    {
        print("Jump");
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
		
		rb.AddForce(orientation.forward * jumpForceForward, ForceMode.Impulse);
        jumpEmmitter.Play();
        if(currentEmmitter != null)
        {
            currentEmmitter.Stop();
        }

        
    }

    private void ResetJump()
    {
        canJump = true;
    }

	private void GroundSlam() {
		// it's like a reverse jump
		rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(-transform.up * slamForce, ForceMode.Impulse);
		//TODO create some push force and particles when you land, e.g. spawn a projectile?
		slamming = true;
		slamLines.SetActive(true);
	}

	private void SlamUpdate() {
		if(isGrounded) {
			slamming = false;
			slamLines.SetActive(false);
		}
	}

    private void SprintCheck()
    {

        //See if we should count down the sprint window
        if (sprintButtonPresses >= 1)
        {
            sprintWindowCounter -= Time.deltaTime;
        }

        if (movementState != MovementState.Crouching)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                sprintButtonPresses++;
                //If we pressed twice in that time frame we are sprinting
                if (sprintButtonPresses >= 2)
                {
                    isSprinting = true;
                }
                else
                {
                    isSprinting = false;
                }
            }

            //Reset if the player realeases forward and its been enough time
            else if (Input.GetKeyUp(KeyCode.W) && sprintWindowCounter <= 0)
            {
                sprintWindowCounter = sprintWindow;
                sprintButtonPresses = 0;
            }
        }
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out raycastSlopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, raycastSlopeHit.normal);
            return angle <= maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMovementAngle()
    {
        return (Vector3.ProjectOnPlane(directionToMove, raycastSlopeHit.normal)).normalized;
    }

    private void PlayerAudioHandler()
    {
        switch(movementState)
        {
            case MovementState.Running:
                {
                   if(lastMovementState != MovementState.Running)
                   {

                   }
                }
                break;
        }

    }

    public void SetSliding(bool sliding)
    {
        
        if(sliding)
        {
            movementState = MovementState.Sliding;
            if(currentEmmitter != null)
            {

            }
            currentEmmitter.Stop();
        }
        else
        {
            if(isGrounded)
            {
                movementState = MovementState.Walking;
            }
            else
            {
                movementState = MovementState.Airborn;
            }
        }
    }
}
