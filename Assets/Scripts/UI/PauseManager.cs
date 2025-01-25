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
            isPaused = false;
        }
        else
        {
            Time.timeScale = 0.0f;
            textMesh.text = "Resume";
            isPaused = true;
        }

        pauseScreen.SetActive(isPaused);
    }

}
