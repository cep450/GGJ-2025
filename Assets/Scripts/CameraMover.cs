using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private GameObject cameraLoc;

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraLoc.transform.position;
    }
}
