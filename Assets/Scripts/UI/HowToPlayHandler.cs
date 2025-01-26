using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayHandler : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject howToPlay;
    private bool howToPlayActive = false;
    public void ToggleHowToPlay()
    {
        howToPlayActive = !howToPlayActive;
        howToPlay.SetActive(howToPlayActive);
        menu.SetActive(!howToPlayActive);
    }
}
