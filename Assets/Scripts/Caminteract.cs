using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Caminteract : MonoBehaviour
{
    public Text InteractionText;

    public bool CanInteract = true;

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

        

        yield return new WaitForSeconds(2f);

    }
}
