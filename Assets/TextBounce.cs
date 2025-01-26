using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBounce : MonoBehaviour
{
    //Fields
    [SerializeField] float startingValue;
    [SerializeField] float staggerRate = 0.2f;
    [SerializeField] float amount = 2f;
    [SerializeField] float speed = 1f;   
    float startingPositionY;

    // Start is called before the first frame update
    void Start()
    {
        startingPositionY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, startingPositionY + (Mathf.Sin((startingValue * staggerRate) + Time.time * speed) * amount), transform.position.z);
        Debug.Log((Mathf.Sin(startingValue + Time.time)));
    }
}
