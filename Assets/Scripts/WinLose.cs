using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLose : MonoBehaviour
{
    private bool gameEnded;

   public void WinLevel()
    {
         if(!gameEnded)
        {
            Debug.Log("You Win!");
            gameEnded = true;
            SceneManager.LoadScene("GameWin");
        }
    }
   
    public void LoseLevel()
    {
        if(!gameEnded)
        {
            Debug.Log("You Lose!");
             SceneManager.LoadScene("GameOver");
            gameEnded = true;
        }
    }
}
