using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXJoint : MonoBehaviour
{
    //Fields
    Rigidbody rb;
    Rigidbody connectedBody = null;
    float force = 200f;


    //Properties
    public Rigidbody ConnectedBody
    {
        get { return connectedBody; }
        set { connectedBody = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(connectedBody)
        {
            Vector3 dir = connectedBody.position - rb.position;
            rb.AddForce((dir * force) - rb.velocity);
        }
    }
}
