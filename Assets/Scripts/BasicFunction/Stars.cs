using UnityEngine;
using System.Collections;

public class Stars : MonoBehaviour
{
    public float timeOff = 5f; // seconds before appearing
    public float timeOn = 5f;  // seconds visible

    void Start()
    {
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        while (true)
        {
            // Hide star
            SetVisible(false);
            yield return new WaitForSeconds(timeOff);

            // Show star
            SetVisible(true);
            yield return new WaitForSeconds(timeOn);
        }
    }

    void SetVisible(bool state)
    {
        // Toggle renderer(s)
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.enabled = state;
        }
    }
}