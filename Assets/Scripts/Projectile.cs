using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the logic for moving an object forward and automatically destroying it
/// </summary>
public class Projectile : MonoBehaviour
{
    #region Fields
    public float lifespan;
    public float speed;
    private float timer;


    [SerializeField] float explosionTimer;
    private bool exploding = false;
    [SerializeField] private LayerMask layersToBomb;

    [SerializeField] private float range;
    [SerializeField] private float maxExplosionRange;
    [SerializeField] float explosionForce;
    #endregion
    // Start is called before the first frame update

    void Start()
    {
       

        //default values for speed and lifespan if not otherwise set
        if (speed == 0)
        {
            speed = 0.05f;
        }
        if (lifespan == 0)
        {
            lifespan = 1.0f;
        }
        if (explosionTimer== 0)
        {
            explosionTimer = 0.5f;
        }

        timer = lifespan;

        //unparent from the current parent
        gameObject.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        //before exploding:
        if (!exploding)
        {
            //move forward
            gameObject.transform.Translate(gameObject.transform.up * -1 * speed, Space.World);

            //update the life timer
            timer -= Time.deltaTime;


            if (timer <= 0)
            {
                Destroy(gameObject);
            }
        }
        
        //while exploding:
        else
        {
            //add an explosion force to all affected objects
            Collider[] hits = Physics.OverlapSphere(transform.position, range, layersToBomb);
            foreach (Collider c in hits)
            {
                float upForce = 1.5f;
                if (c.CompareTag("Player")) upForce = 0;
                c.attachedRigidbody.AddExplosionForce(explosionForce, gameObject.transform.position, 0.0f, upForce, ForceMode.Impulse);
            }

            explosionTimer -= Time.deltaTime;
            if (explosionTimer <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        
    }

    /// <summary>
    /// spawn a bomb at the hit location
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        print("Entered: " + other.name);
        if (other.CompareTag("Player")) return;
        
        exploding = true;

    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Collided with " + collision.gameObject.name);
    }

    private void OnDestroy()
    {
        GameObject bomb = Instantiate(new GameObject("bomb"));
        bomb.AddComponent<Bomb>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
