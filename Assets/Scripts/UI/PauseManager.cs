using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    [SerializeField] private GameObject pauseScreen;
	[SerializeField] private GameObject controlsReference;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }

		// controls reference 
		if(Input.GetKey(KeyCode.Tab)) {
			controlsReference.SetActive(true);
		} else {
			controlsReference.SetActive(false);
		}
    }

    public void TogglePause()
    {
        if(isPaused)
        {
            print("Unpaused");
            Time.timeScale = 1.0f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isPaused = false;
        }
        else
        {
            print("Paused");
            Time.timeScale = 0.0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isPaused = true;
        }

        pauseScreen.SetActive(isPaused);
    }

}
