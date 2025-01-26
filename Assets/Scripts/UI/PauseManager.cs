using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    [SerializeField] private GameObject pauseScreen;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
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
