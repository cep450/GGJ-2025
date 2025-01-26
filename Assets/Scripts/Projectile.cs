using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the logic for moving an object forward and automatically destroying it
/// </summary>
public class Projectile : MonoBehaviour
{
    public float lifespan;
    public float speed;
    private float timer;
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

        timer = lifespan;
    }

    // Update is called once per frame
    void Update()
    {
        //move forward
        gameObject.transform.Translate(gameObject.transform.up * -1 * speed, Space.World);

        //update the life timer
        timer -= Time.deltaTime;


        if(timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
