using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Collectible : MonoBehaviour
{
    public static event Action OnCollected;
    //public static int total;

    [Header("Audio")]
    public AudioClip woodCollectSound;

    void Awake()
    {
     //   total++;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Fire collection event
            OnCollected?.Invoke();

            // Play sound at this object's position
            if (woodCollectSound != null)
            {
                AudioSource.PlayClipAtPoint(
                    woodCollectSound,
                    transform.position
                );
            }

            // Destroy collectible
            Destroy(gameObject);
        }
    }
}