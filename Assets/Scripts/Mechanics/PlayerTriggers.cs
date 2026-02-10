using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggers : MonoBehaviour
{
    public WinLose winLoseScript;
   public int count;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10.0f)
        {
            winLoseScript.LoseLevel();
        }
        
        if (count > 4 )
        {
            winLoseScript.WinLevel();
        }
    }



         void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            winLoseScript.LoseLevel();
        }
    }

    
    
}
