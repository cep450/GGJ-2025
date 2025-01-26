using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBounce : MonoBehaviour
{
    //Fields
    [SerializeField] float startingValue;
    float startingPositionY;

    // Start is called before the first frame update
    void Start()
    {
        startingPositionY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, startingPositionY + (Mathf.Sin(startingValue + Time.time) * 2f), transform.position.z);
        Debug.Log((Mathf.Sin(startingValue + Time.time)));
    }
}
