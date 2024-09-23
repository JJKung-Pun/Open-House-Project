using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
   public void RespawnGame()
   {
	   SceneManager.LoadSceneAsync(2);
   }
}