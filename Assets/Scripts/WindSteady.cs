using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSteady : MonoBehaviour
{
    //fields for calculating detection


    [SerializeField] float forceMagnitude;

    [SerializeField] LayerMask terrainMask;
    [SerializeField] LayerMask blowableMask;
    private int collisionBufferCap = 2;
    private int collisionBufferCounter = 0;

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



    private void OnTriggerStay(Collider other)
    {
        print("Colliding with " + other.gameObject.name);
        //only try to move blowable objects
        if(other.gameObject.tag == "Blowable")
        {
            print("Blowing");
            //for each object that should be blown, calculate magnitude and direction of the force
            //apply force
            collisionBufferCounter++;
            if(collisionBufferCounter >= collisionBufferCap)
            {
                collisionBufferCounter = 0;
                Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

                //rb.Addforce(Vector3 force);
                Debug.Log("Wind Force " + (gameObject.transform.up.normalized + new Vector3(0, -8.4f, 0)));
                rb.AddForce((gameObject.transform.up.normalized + new Vector3(0, -8.4f, 0)) * -1 * forceMagnitude);
            }

        }
        
    }

    

}
