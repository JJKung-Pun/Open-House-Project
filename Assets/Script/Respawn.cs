using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
	public string sceneToLoad = "Level1"; 
   public void RespawnGame()
   {
	   SceneManager.LoadSceneAsync(sceneToLoad);
   }
}