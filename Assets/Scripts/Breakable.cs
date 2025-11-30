using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] GameObject fullTree;
    [SerializeField] GameObject treeChopped;
    // Define the required proximity radius
    [SerializeField] float interactionRadius = 3f;

    private BoxCollider boxCollider;
    private GameObject woodAxe; // Reference to the WoodAxe object

    private void Awake()
    {
        // Set initial state and get the BoxCollider component
        fullTree.SetActive(true);
        treeChopped.SetActive(false);
        boxCollider = GetComponent<BoxCollider>();

        // Find the GameObject tagged "WoodAxe" in the scene once
        // (Ensure an object with this tag exists in your scene)
        woodAxe = GameObject.FindGameObjectWithTag("WoodAxe");

        if (woodAxe == null)
        {
            Debug.LogError("GameObject with tag 'WoodAxe' not found in the scene!");
        }
    }

    private void OnMouseDown()
    {
        // Check if the WoodAxe object exists and if the player is within the interaction radius
        if (woodAxe != null)
        {
            float distance = Vector3.Distance(transform.position, woodAxe.transform.position);

            if (distance <= interactionRadius)
            {
                // Call the Break method only if in proximity
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
        // Disable the full tree, enable the chopped tree, and disable the collider
        fullTree.SetActive(false);
        treeChopped.SetActive(true);
        boxCollider.enabled = false;
        Debug.Log("Tree broken!");
    }

    // Optional: Draw the interaction radius in the editor for visualization
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}