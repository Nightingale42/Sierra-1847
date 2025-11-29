using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class Caminteract : MonoBehaviour
{

    //Lookat Check

    public bool TalkToActualFriend = false;
    //look at animation 

    public bool TalkToRedFriend = false;
    //look at animation 

    //Lookat Check
    public LookAtFunction LookAtScript;

    public Text InteractionText;

    
    private float InteractDistance = 2f;

    public bool CanInteract = true;
    
    

    //look at

    public CinemachineVirtualCamera PlayerVcam;
    public CinemachineVirtualCamera TalkZoomVcam;

    public CinemachineVirtualCamera RedFriendZoomVcam;

    public PlayerMovement FpsController;

    //talk
        public GameObject TalkPanel; 

        public GameObject ChoicePack;

        public Text SubText;
        string holder;
        float time = 0.05f;

    //talk



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(CanInteract == true)
        {
            
            Ray ray1 = new Ray(transform.position, transform.forward);
            RaycastHit hit1;
            
            if(Physics.Raycast(ray1, out hit1, InteractDistance))
            {
                if (hit1.collider.CompareTag("Friend"))
                {
                    InteractionText.text = "Talk To Him";
                    //talk to him
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        CanInteract = false;
                        StartCoroutine(TalkToFriendCO());
                    }
                }
                //talk to him

             else if (hit1.collider.CompareTag("RedFriend"))
            {
                InteractionText.text = "Talk to red Friend";
                //talk to him
                
                if (Input.GetKeyDown(KeyCode.E))
                {
                    CanInteract = false;
                    StartCoroutine(TalkToRedFriendCO());

                }

                //talk to him

            }

                else 
                {
                    InteractionText.text = "";
                }
            }
           

            
            else 
            {

                InteractionText.text = "";

            }
        }
    
    }

    IEnumerator TalkToRedFriendCO()
    {
        //check bool 

        TalkToRedFriend = true;

        //check bool
        InteractionText.text= "";
        FpsController.enabled= false;
        //wont be able to move
        RedFriendZoomVcam.enabled = true;
        PlayerVcam.enabled = false;
        TalkZoomVcam.enabled = false;
        
        //look at

        LookAtScript.IKActive = true;

        //look at
        yield return new WaitForSeconds(1f);

        TalkPanel.SetActive(true);

           SubText.text = "Me: ";
        holder = "How're you holding up kid?";
        foreach(char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);

        }

        yield return MousePress();

         SubText.text = "Kid: ";
        holder = "Not great I guess... It's so cold out and my stomach hurts. I haven't eaten in days.";
        foreach(char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);

        }

        yield return MousePress();
        
        StartCoroutine(FinalCO());



    }


    IEnumerator TalkToFriendCO()
    {
        //check bool

        TalkToActualFriend = true;
        

        //check bool

        InteractionText.text = "";
        FpsController.enabled = false;
        TalkZoomVcam.enabled = true;
        PlayerVcam.enabled = false;
        RedFriendZoomVcam.enabled = false;

        //look at
        LookAtScript.IKActive = true;
        //look at

        yield return new WaitForSeconds(1f);

        FpsController.enabled = false;


        //cursor
         Cursor.lockState = CursorLockMode.None;
         Cursor.visible = true;
        //cursor

        TalkPanel.SetActive(true);

        SubText.text = "Me: ";
        holder = "Hey man, can I help with anything?";
        foreach(char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);

        }

        yield return MousePress();

              SubText.text = "Friend: ";
        holder = "Hey there. We could use some help collecting wood for the cabins. ";
        foreach(char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);

        }


        yield return MousePress();

              SubText.text = "Friend: ";
        holder = "The Axe is over by the wagon. Are you in?";
        foreach(char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);

        }

        yield return new WaitForSeconds(1f);

        ChoicePack.SetActive(true);
        
        

    }
    
    public void Choice1Void()
    {
        StartCoroutine(Choice1CO());
    }

    public void Choice2Void()
    {

        StartCoroutine(Choice2CO());

    }





    IEnumerator Choice1CO()
    {
        ChoicePack.SetActive(false);
    
         SubText.text = "Me: ";
        holder = "Yeah I can help out with that.";
        foreach(char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
            

        }
        
        yield return new WaitForSeconds(2f);
        
        

   StartCoroutine(FinalCO());

    }






     IEnumerator Choice2CO()
    {

        ChoicePack.SetActive(false);

            SubText.text = "Me: ";
        holder = "No, sorry I'm busy at the moment.";
        foreach(char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);

        }

        yield return new WaitForSeconds(2f);
      

      StartCoroutine(FinalCO());

        




        }

        IEnumerator FinalCO()
        {

            //Check  bool

            TalkToRedFriend = false;
            TalkToActualFriend = false;

            //reset everything for head turn animation

            //Check bool
            TalkPanel.SetActive(false);
            FpsController.enabled = true;
            ChoicePack.SetActive(false);
            SubText.text = "";
            //look at 


             Cursor.visible = false;





        LookAtScript.IKActive = false;
        //look at

        FpsController.enabled = true;
        PlayerVcam.enabled = true;
        TalkZoomVcam.enabled = false;
        RedFriendZoomVcam.enabled = false;

        CanInteract= true;

        Cursor.lockState = CursorLockMode.Locked;

        yield return null;
    }


IEnumerator MousePress()
{
    Debug.Log("Waiting for mouse press...");
    while(!Input.GetMouseButtonDown(0))
        yield return null;
    Debug.Log("Mouse pressed!");
}






}
