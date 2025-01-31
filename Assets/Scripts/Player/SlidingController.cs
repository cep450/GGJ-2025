using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SlidingController : MonoBehaviour
{
    #region Fields
    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform playerTransform;
	[SerializeField] private GameObject speedLines;
    private Vector3 slideDirection;
    private Rigidbody rb;
    private PlayerController pc;

    [Header("Sliding Logic")]
    [SerializeField] private float maxSlideTime;
    private float slideTime;
    [SerializeField] private float slideForce;

    [SerializeField] private float slideHeight;
    private float normalHeight;

    private bool isSliding;

    [Header("Movement Logic")]
    private float horizontalInput;
    private float verticalInput;

    [SerializeField] private StudioEventEmitter slidingEmmitter;

    //I dont feel like going back and reediting everything if I thought of the wrong key so here we are
    private KeyCode slideKey = KeyCode.LeftShift;
    #endregion Fields
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pc = GetComponent<PlayerController>();
        normalHeight = transform.localScale.y;
        slideTime = maxSlideTime;

		speedLines = GameObject.FindGameObjectWithTag("SpeedLines");
		speedLines.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        /*
        Debug.Log("sliding = " + Input.GetKeyDown(slideKey));
        Debug.Log("Inputing = " + (verticalInput != 0 || horizontalInput != 0));
        Debug.Log("Sprint Key = " + pc.isSprinting);*/
        if(Input.GetKeyDown(slideKey) && (verticalInput != 0 || horizontalInput != 0) && pc.isSprinting)
        {
            StartSlide();
        }
        if(Input.GetKeyUp(slideKey) && isSliding)
        {
            StopSlide();
        }
        
    }

    private void FixedUpdate()
    {
        if(isSliding)
        {
            Sliding();
        }
    }

    private void StartSlide()
    {
        isSliding = true;
        transform.localScale = new Vector3(transform.localScale.x, slideHeight, transform.localScale.z);
        //Push player down a bit when they slide
        rb.AddForce(Vector3.down * 5.0f, ForceMode.Impulse);
		speedLines.SetActive(true);
        pc.SetSliding(true);
        slidingEmmitter.Play();
    }

    private void Sliding()
    {
        slideDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(slideDirection * Time.fixedDeltaTime * slideForce, ForceMode.Force);
        slideTime -= Time.deltaTime;
        if(slideTime <= 0)
        {
            StopSlide();
        }
    }

    private void StopSlide()
    {
        isSliding = false;
        slideTime = maxSlideTime;
        transform.localScale = new Vector3(transform.localScale.x, normalHeight, transform.localScale.z);
		speedLines.SetActive(false);
        pc.SetSliding(false);
        slidingEmmitter.Stop();
    }

    private void Inputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }
}
