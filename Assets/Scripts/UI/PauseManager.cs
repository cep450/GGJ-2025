using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] TextMeshProUGUI textMesh;

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
            Time.timeScale = 1.0f;
            textMesh.text = "Pause";
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            isPaused = false;
        }
        else
        {
            Time.timeScale = 0.0f;
            textMesh.text = "Resume";
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isPaused = true;
        }

        pauseScreen.SetActive(isPaused);
    }

}
