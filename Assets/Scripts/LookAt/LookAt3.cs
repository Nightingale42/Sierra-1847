using UnityEngine;

public class LookAt3 : MonoBehaviour
{
    

    public Animator animator;
    public Animator RedFriendAnimator;
    public Animator BlueFriendAnimator;
    public Animator GreenFriendAnimator;
     public Animator OrangeFriendAnimator;
    public Animator PinkFriendAnimator;
    public Animator YellowFriendAnimator;
    public Animator PurpleFriendAnimator;

    public bool IKActive = false;

    public Transform LookAtObj = null;

    public float LookWeight = 0f;


    public D2Caminteract D2CamInteract;


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



            if(D2CamInteract.TalkToActualFriend == true)
            {

                animator.SetLookAtWeight(LookWeight);
                animator.SetLookAtPosition(LookAtObj.position);


            }
            else if(D2CamInteract.TalkToRedFriend == true)
            {

                RedFriendAnimator.SetLookAtWeight(LookWeight);
                RedFriendAnimator.SetLookAtPosition(LookAtObj.position);


            }

             else if(D2CamInteract.TalkToBlueFriend == true)
            {

                BlueFriendAnimator.SetLookAtWeight(LookWeight);
                BlueFriendAnimator.SetLookAtPosition(LookAtObj.position);


            }

             else if(D2CamInteract.TalkToGreenFriend == true)
            {

                GreenFriendAnimator.SetLookAtWeight(LookWeight);
                GreenFriendAnimator.SetLookAtPosition(LookAtObj.position);


            }


              else if(D2CamInteract.TalkToOrangeFriend == true)
            {

                OrangeFriendAnimator.SetLookAtWeight(LookWeight);
                OrangeFriendAnimator.SetLookAtPosition(LookAtObj.position);


            }


             else if(D2CamInteract.TalkToYellowFriend == true)
            {

                YellowFriendAnimator.SetLookAtWeight(LookWeight);
                YellowFriendAnimator.SetLookAtPosition(LookAtObj.position);


            }


             else if(D2CamInteract.TalkToPinkFriend == true)
            {

                PinkFriendAnimator.SetLookAtWeight(LookWeight);
                PinkFriendAnimator.SetLookAtPosition(LookAtObj.position);


            }

                else if(D2CamInteract.TalkToPurpleFriend == true)
            {

                PurpleFriendAnimator.SetLookAtWeight(LookWeight);
                PurpleFriendAnimator.SetLookAtPosition(LookAtObj.position);


            }



            



        }

        



    }




}