using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
   public void NewGame()
   {

	   SceneManager.LoadSceneAsync(1);
   }
   public void LoadGame()
   {
	   SceneManager.LoadSceneAsync(2);
   }
   public void QuitGame()
   {
	   Application.Quit();
   }
}
