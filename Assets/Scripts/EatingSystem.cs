using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingSystem : MonoBehaviour
{
    private bool CanInteract = true;
    public static int SliceEaten = 0;
    [SerializeField] private GameObject[] soupbowls;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip eatingclip;

    void Update()
    {
        if (!CanInteract)
            return;

        // Block eating until player has collected more than 2 items
        if (Caminteract.count <= 2)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 5f))
            {
                if (hit.collider.CompareTag("BowlSoup"))
                {
                    if (SliceEaten >= soupbowls.Length)
                        return;

                    StartCoroutine(EatSoupCO());
                }
            }
        }
    }

    IEnumerator EatSoupCO()
    {
        CanInteract = false;

        source.PlayOneShot(eatingclip);

        if (SliceEaten < soupbowls.Length)
        {
            soupbowls[SliceEaten].SetActive(false);
            SliceEaten++;
        }

        yield return new WaitForSeconds(.001f);

        CanInteract = true;
    }
}