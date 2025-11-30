using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour
{ 
    public Camera fpsCam; 
    public ParticleSystem muzzleFlash; 
    public GameObject impactEffect; 
    public float damage = 10f; 
    public float range = 100f; 
    public float fireRate = 1f; 
    public float maxShootDistanceToPlayer = 2f; // Set your desired maximum distance here

    private float nextTimeToFire = 2f; 
    private GameObject playerObject; // Reference to the player object

    void Start()
    {
        // Find the GameObject with the "Player" tag
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null)
        {
            Debug.LogError("Player object with 'Player' tag not found!");
        }
    }

    void Update() 
    { 
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire) 
        { 
            // Check if a player object exists and if the distance is within limits
            if (playerObject != null && Vector3.Distance(transform.position, playerObject.transform.position) <= maxShootDistanceToPlayer)
            {
                nextTimeToFire = Time.time + 5f / fireRate; 
                Shoot(); 
            }
            else if (playerObject == null)
            {
                Debug.LogWarning("Cannot shoot: Player object not found.");
            }
            else
            {
                Debug.Log("Cannot shoot: Too far from player.");
            }
        } 
    }

    void Shoot () 
    { 
        muzzleFlash.Play(); 
        RaycastHit hit; 
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)) 
        { 
            Debug.Log(hit.transform.name); 
            Target target = hit.transform.GetComponent<Target>(); 
            if (target != null) 
            { 
                target.TakeDamage(damage); 
            } 
            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal)); 
            Destroy(impactGO, 2f); 
        } 
    }
}