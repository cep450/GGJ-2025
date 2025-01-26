using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private GameObject cameraLoc;
	public static bool moveCamera = true;

	void Start() {
		moveCamera = true;
	}

    // Update is called once per frame
    void Update()
    {
		if(!moveCamera) return; 

        transform.position = cameraLoc.transform.position;
    }
}
