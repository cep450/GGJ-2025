using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingController : MonoBehaviour
{
    #region Fields
    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform playerTransform;
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

    //I dont feel like going back and reediting everything if I thought of the wrong key so here we are
    private KeyCode slideKey = KeyCode.C;
    #endregion Fields
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pc = GetComponent<PlayerController>();
        normalHeight = transform.localScale.y;
        slideTime = maxSlideTime;
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        if(Input.GetKeyDown(slideKey) && (verticalInput != 0 || horizontalInput != 0))
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
        print("YIPEEE");
        isSliding = true;
        transform.localScale = new Vector3(transform.localScale.x, slideHeight, transform.localScale.z);
        //Push player down a bit when they slide
        rb.AddForce(Vector3.down * 5.0f, ForceMode.Impulse);
    }

    private void Sliding()
    {
        slideDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(slideDirection * Time.deltaTime * slideForce, ForceMode.Force);
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
    }

    private void Inputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }
}
