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

    private float nextTimeToFire = 2f;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 5f / fireRate;
            Shoot();
        }
        
    }
////reminder that bracky's shooting with raycasts video has logic on animations for hit/ killed objects!!! use it!
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
