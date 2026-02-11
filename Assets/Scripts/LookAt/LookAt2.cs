using UnityEngine;

public class LookAt2 : MonoBehaviour
{
    

    public Animator animator;

    public Animator RedFriendAnimator;

    public Animator BlueFriendAnimator;
    
    public Animator PinkFriendAnimator;

    public Animator OrangeFriendAnimator;

    public bool IKActive = false;

    public Transform LookAtObj = null;

    public float LookWeight = 0f;


    public Caminteract CamInteract;


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



            if(CamInteract.TalkToActualFriend == true)
            {

                animator.SetLookAtWeight(LookWeight);
                animator.SetLookAtPosition(LookAtObj.position);


            }
            else if(CamInteract.TalkToRedFriend == true)
            {

                RedFriendAnimator.SetLookAtWeight(LookWeight);
                RedFriendAnimator.SetLookAtPosition(LookAtObj.position);


            }

            else if(CamInteract.TalkToBlueFriend == true)
            {

                BlueFriendAnimator.SetLookAtWeight(LookWeight);
                BlueFriendAnimator.SetLookAtPosition(LookAtObj.position);


            }

             else if(CamInteract.TalkToPinkFriend == true)
            {

                PinkFriendAnimator.SetLookAtWeight(LookWeight);
                PinkFriendAnimator.SetLookAtPosition(LookAtObj.position);


            }

             else if(CamInteract.TalkToOrangeFriend == true)
            {

                OrangeFriendAnimator.SetLookAtWeight(LookWeight);
                OrangeFriendAnimator.SetLookAtPosition(LookAtObj.position);


            }
            



        }

        



    }




}