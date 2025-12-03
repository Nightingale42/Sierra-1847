using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingSystem : MonoBehaviour
{

    private bool CanInteract = true;
    private int SliceEaten = 0; //you're probably going to need to refrence this later. May need to change to public static int!
    [SerializeField] private GameObject[] soupbowls;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip eatingclip; //you might have to change this to the name of the clip




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (CanInteract == true)
        {

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 5f))
                {

                    if(hit.collider.CompareTag("BowlSoup"))
                    {

                        if(SliceEaten >= 6)
                        {
                            return;
                        }

                        StartCoroutine(EatSoupCO());

                    }

                }
            }


        }

        
    }
        IEnumerator EatSoupCO()
        {

            CanInteract= false;
            //yer busy

            source.PlayOneShot(eatingclip); //might need 2 change this

            if (SliceEaten == 0)
            {
                SliceEaten = 1;
                soupbowls[0].SetActive(false); //if you have eaten one slice before it eats slice two and so on. logic can probs be used elsewhere too
            }
            else if (SliceEaten == 1)
            {
                SliceEaten = 2;
                soupbowls[1].SetActive(false);
            }
            else if (SliceEaten == 2)
            {
                SliceEaten = 3;
                soupbowls[2].SetActive(false);
            }
            else if (SliceEaten == 3)
            {
                SliceEaten = 4;
                soupbowls[3].SetActive(false);
            }
            else if (SliceEaten == 4)
            {
                SliceEaten = 5;
                soupbowls[4].SetActive(false);
            }
            else if (SliceEaten == 5)
            {
                SliceEaten = 6;
                soupbowls[5].SetActive(false);
            }

            yield return new WaitForSeconds(.001f);

            CanInteract = true;
        }
}
