using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject gameOverCanvas;
    [Header("Gravity Fields")]
    [SerializeField] Vector3 gravityVector;
    [SerializeField] ConstantForce constantForce;
    private float randomMovement = 1.05f;
    #endregion Fields
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        SomewhatRealisticGravity();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(true == false)
        {

        }
        else
        {
            //Insert Logic for goal and player here
            Popped();
        }

    }

    private void Popped()
    {
        //Insert Pop VFX
        gameObject.SetActive(false);
        //Insert Retry Level Pop Up
        gameOverCanvas.SetActive(true);
        
    }

    private void SomewhatRealisticGravity()
    {
        float randoX = Random.value * randomMovement - (randomMovement / 2);
        float randoZ = Random.value * randomMovement - (randomMovement / 2);
        constantForce.relativeForce = gravityVector + new Vector3(randoX, 0, randoZ);
    }
}
