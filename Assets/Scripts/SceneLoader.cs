using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

	public enum Scene {
		MENUS, GAMEPLAY
	}

	private static Scene currentScene = Scene.MENUS;
	public static Scene CurrentScene { get {
		return currentScene;
	} private set { } }


    // Start is called before the first frame update
    void Start()
    {
        
    }


	public static void LoadGameplay() {
		
		SceneManager.LoadScene("Game");
		currentScene = Scene.GAMEPLAY;
	}

	public static void ReloadLevel() {

	}

	public static void LoadMainMenu() {

		SceneManager.LoadScene("Menus");
		currentScene = Scene.MENUS;
	}

	public static void ExitGame() {

	}


}
