using UnityEngine;

public class LookAt4 : MonoBehaviour
{
    

    public Animator animator;

    public Animator RedFriendAnimator;

    public bool IKActive = false;

    public Transform LookAtObj = null;

    public float LookWeight = 0f;


    public D3Caminteract D3CamInteract;


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



            if(D3CamInteract.TalkToActualFriend == true)
            {

                animator.SetLookAtWeight(LookWeight);
                animator.SetLookAtPosition(LookAtObj.position);


            }
            else if(D3CamInteract.TalkToRedFriend == true)
            {

                RedFriendAnimator.SetLookAtWeight(LookWeight);
                RedFriendAnimator.SetLookAtPosition(LookAtObj.position);


            }

            



        }

        



    }




}