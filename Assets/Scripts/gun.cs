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
    public float maxShootDistanceToPlayer = 2f;

    private float nextTimeToFire = 2f;
    private GameObject playerObject;

    private AudioSource audioSource; // 🔊 Audio source for gunshot sound

    void Start()
    {
        // Get the AudioSource attached to this GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found on this gun object!");
        }

        // Find the player
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

    void Shoot()
    {
        // 🔊 Play gunshot sound
        if (audioSource != null)
            audioSource.Play();

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