using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

	public enum Scene {
		MENUS, GAMEPLAY, CREDITS
	}

	private static Scene currentScene = Scene.MENUS;
	public static Scene CurrentScene { get {
		return currentScene;
	} private set { } }


	public static void LoadGameplay() 
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene("MallTest");
		currentScene = Scene.GAMEPLAY;
	}

	public static void LoadCredits()
	{
		SceneManager.LoadScene("Credits");
		currentScene = Scene.CREDITS;
	}

	public static void ReloadLevel() 
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public static void LoadMainMenu() {

		SceneManager.LoadScene("TitleScreen3D");
		currentScene = Scene.MENUS;
	}

	public static void ExitGame() {
		Application.Quit();
	}


}
