using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blower : MonoBehaviour
{
    public Camera playerCam;
    //fields for calculating detection
    public Vector3 origin;
    [SerializeField] float radius;
    [SerializeField] Quaternion direction;
    [SerializeField] float range;
    [SerializeField] GameObject windOrigin;
    [SerializeField] GameObject windCollider;

    [SerializeField] float forceMagnitude;


    [SerializeField] LayerMask terrainMask;
    [SerializeField] LayerMask blowableMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //update position and rotation
        //origin = windOrigin.transform.position;
        //direction = windOrigin.transform.rotation;

        //check if any objects should be blown
        

        //for each object that should be blown, calculate magnitude and direction of the force
        //apply force

        //for debugging purposes before player controller is implemented
        //if (Input.GetKey(KeyCode.M))
        //{
        //    gameObject.transform.Rotate(new Vector3(0.3f, 0, 0));
        //}
    }



    public void CheckforObject()
    {

    }


    private void OnCollisionStay(Collision collision)
    {
        print(collision.gameObject.name);
    }

    private void OnTriggerStay(Collider other)
    {
        //only try to move blowable objects
        if(other.gameObject.tag == "Blowable")
        {
            print(other.gameObject.name);

            //for each object that should be blown, calculate magnitude and direction of the force
            //apply force

            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

            //rb.Addforce(Vector3 force);
            rb.AddForce(gameObject.transform.up * -1 * forceMagnitude);
        }
        
    }



}
