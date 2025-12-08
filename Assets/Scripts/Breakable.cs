using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] GameObject fullTree;
    [SerializeField] GameObject treeChopped;
    [SerializeField] float interactionRadius = 3f;

    private BoxCollider boxCollider;
    private GameObject woodAxe; 
    private AudioSource audioSource; // 🔊 Audio source for break sound

    private void Awake()
    {
        // Get the AudioSource attached to this object
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found on this Breakable object!");
        }

        fullTree.SetActive(true);
        treeChopped.SetActive(false);
        boxCollider = GetComponent<BoxCollider>();

        woodAxe = GameObject.FindGameObjectWithTag("WoodAxe");

        if (woodAxe == null)
        {
            Debug.LogError("GameObject with tag 'WoodAxe' not found in the scene!");
        }
    }

    private void OnMouseDown()
    {
        if (woodAxe != null)
        {
            float distance = Vector3.Distance(transform.position, woodAxe.transform.position);

            if (distance <= interactionRadius)
            {
                Break();
            }
            else
            {
                Debug.Log("You are too far from the WoodAxe to break the tree!");
            }
        }
    }

    private void Break()
    {
        // 🔊 Play break sound
        if (audioSource != null)
            audioSource.Play();

        fullTree.SetActive(false);
        treeChopped.SetActive(true);
        boxCollider.enabled = false;

        Debug.Log("Tree broken!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}