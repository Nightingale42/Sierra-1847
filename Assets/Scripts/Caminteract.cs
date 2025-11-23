using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class Caminteract : MonoBehaviour
{
    public LookAtFunction LookAtScript;

    public Text InteractionText;

    public bool CanInteract = true;
    

    //look at

    public CinemachineVirtualCamera PlayerVcam;
    public CinemachineVirtualCamera TalkZoomVcam;

    public PlayerMovement FpsController;

    //talk
        public GameObject TalkPanel; 

        public GameObject ChoicePack;

        public Text SubText;
        string holder;
        float time = 0.05f;

    //talk


    private float InteractDistance = 2f;

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
        else
        {
            InteractionText.text = "";
        }
    }
    IEnumerator TalkToFriendCO()
    {
        InteractionText.text = "";
        FpsController.enabled = false;
        TalkZoomVcam.enabled = true;
        PlayerVcam.enabled = false;

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
        holder = "Are you in?";
        foreach(char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);

        }

        yield return new WaitForSeconds(1f);

        ChoicePack.SetActive(true);



        yield return new WaitForSeconds(5f);

        //look at 

        LookAtScript.IKActive = false;
        //look at

        FpsController.enabled = true;
        PlayerVcam.enabled = true;
        TalkZoomVcam.enabled = false;

        CanInteract= true;


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
        holder = "Yes";
        foreach(char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);

        }

   StartCoroutine(FinalCO());
    }

     IEnumerator Choice2CO()
    {

        ChoicePack.SetActive(false);

            SubText.text = "Me: ";
        holder = "No";
        foreach(char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);

        }

      StartCoroutine(FinalCO());

        


        }

        IEnumerator FinalCO()
        {
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

        CanInteract= true;
        Cursor.lockState = CursorLockMode.Locked;

        yield return null;
    }



        IEnumerator MousePress()
        {
            while(!Input.GetMouseButtonDown(0))
            {
                yield return null;
            }
        }






}
