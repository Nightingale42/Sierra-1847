using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour

{
    [SerializeField] GameObject fullTree;
    [SerializeField] GameObject treeChopped;

    private BoxCollider boxCollider;

    private void Awake()
    {
        // Set initial state and get the BoxCollider component
        fullTree.SetActive(true);
        treeChopped.SetActive(false);
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnMouseDown()
    {
        // Call the Break method when the object is clicked
        Break();
    }

    private void Break()
    {
        // Disable the full tree, enable the chopped tree, and disable the collider
        fullTree.SetActive(false);
        treeChopped.SetActive(true);
        boxCollider.enabled = false;
    }
}
