using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class Caminteract : MonoBehaviour
{
    public Text InteractionText;

    public bool CanInteract = true;

    //look at

    public CinemachineVirtualCamera PlayerVcam;
    public CinemachineVirtualCamera TalkZoomVcam;

    public PlayerMovement FpsConroller;


    private float InteractDistance = 5f;

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
        FpsConroller.enabled = false;
        TalkZoomVcam.enabled = true;
        PlayerVcam.enabled = false;

        yield return new WaitForSeconds(5f);

        FpsConroller.enabled = true;
        PlayerVcam.enabled = true;
        TalkZoomVcam.enabled = false;

        CanInteract= true;


    }
}
