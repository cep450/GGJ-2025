using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WindType
{
    Steady,
    Blast,
    Standby,
    Cooldown
}
public class Blower : MonoBehaviour
{
    public GameObject windSteady;
    public GameObject windBlast;
    [SerializeField] Transform spawnTransform;

    public Input blowInput;
    public Input burstInput;

    private GameObject wind;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //state machine that handles the type of wind the player wants to shoot

        //if press space, spawn a steady wind
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnSteadyWind();
        }
        //if release space, despawn the steady wind
        if (Input.GetKeyUp(KeyCode.P))
        {
            GameObject.Destroy(wind);
        }

        //if press right key, spawn burst wind
        if (Input.GetKeyDown(KeyCode.B))
        {
            SpawnBurstWind();
        }
    }

    /// <summary>
    /// Spawns a steady wind object and returns it
    /// </summary>
    /// <returns></returns>
    public GameObject SpawnSteadyWind()
    {
        wind = Instantiate(windSteady, spawnTransform);
        return wind;
    }

    public void SpawnBurstWind()
    {
        Instantiate(windBlast, spawnTransform);
    }

    /// <summary>
    /// Destroys the wind object
    /// </summary>
    public void DestroyWind()
    {
        GameObject.Destroy(wind);
    }
}
