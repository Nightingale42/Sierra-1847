using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtFunction : MonoBehaviour
{
public Animator animator;

public Animator RedFriendAnimator;

public bool IKActive = false;

public Transform LookAtObj = null; 

public float LookWeight = 2f;

public Caminteract CamInteraction;

private void OnAnimatorIK(int layerIndex)
{
    if (this.gameObject.GetComponent<Animator>())
    {
        if(IKActive)
        {
            if (LookAtObj != null)
            {
                LookWeight = Mathf.Lerp(LookWeight, 1, Time.deltaTime * 2);

                
            }
        }
        else
        {
            LookWeight = Mathf.Lerp(LookWeight, 0, Time.deltaTime * 2);
         
        }

            if(CamInteraction.TalkToActualFriend == true)
            {
                
                animator.SetLookAtWeight(LookWeight);
                animator.SetLookAtPosition(LookAtObj.position);
            }
            else if (CamInteraction.TalkToRedFriend == true)
            {
                 
                RedFriendAnimator.SetLookAtWeight(LookWeight);
                RedFriendAnimator.SetLookAtPosition(LookAtObj.position);
            }


    }
}
}
