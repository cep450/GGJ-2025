using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bubble : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject gameOverCanvas;
    [Header("Gravity Fields")]
    [SerializeField] Vector3 gravityVector;
    [SerializeField] ConstantForce constantForce;
    private float randomMovement = 0.0f; //50.05f;
    [SerializeField] private GoalManager goalManager;
    [SerializeField] private float driftTimer;
    private float driftCounter;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private TextMeshProUGUI scoreText;
    #endregion Fields
    private void FixedUpdate()
    {
        //Debug.Log(rb.velocity.magnitude);
        SomewhatRealisticGravity();
        driftCounter -= Time.fixedDeltaTime;
        if(driftCounter <= 0)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
        rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -1, 0), rb.velocity.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "GoalPoint")
        {
            goalManager.SetNewGoalPoint();
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
        Cursor.lockState = CursorLockMode.Confined;
        gameOverCanvas.SetActive(true);
        scoreText.text = $"Final Score: {goalManager.score}";
    }

    private void SomewhatRealisticGravity()
    {
        float randoX = Random.value * randomMovement - (randomMovement / 2);
        float randoZ = Random.value * randomMovement - (randomMovement / 2);
        constantForce.relativeForce = gravityVector + new Vector3(randoX, 0, randoZ);
    }
}
