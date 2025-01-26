using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private LayerMask layersToHit;
    public float explosionForce;

    // Start is called before the first frame update
    void Start()
    {
        range = 1000.0f;
        explosionForce = 5000.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void OnTriggerEnter(Collider other)
    {
        

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, range);
    }




}
