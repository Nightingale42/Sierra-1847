using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SleepSystem : MonoBehaviour
{
    [Header("Interaction")]
    public bool CanInteract = true;
    [SerializeField] private GameObject BlackPanel_GO;

    [Header("Movement Script")]
    public PlayerMovement playerMovement; // reference to the PlayerMovement script

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI Subtext; 
    private string Holder;
    private float WriteSpeed = 0.5f;

    private void Start()
    {
        // If you forgot to assign via inspector, try finding it automatically
        if (playerMovement == null)
        {
            playerMovement = FindObjectOfType<PlayerMovement>();
            if (playerMovement == null)
                Debug.LogError("PlayerMovement script not found in scene!");
        }
    }

    private void Update()
    {
        if (!CanInteract) return;

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5f))
        {
            if (hit.collider.CompareTag("Bed") && Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(SleepCO());
            }
        }
    }

    IEnumerator SleepCO()
    {
        BlackPanel_GO.SetActive(true);
        CanInteract = false;

        // Disable player movement
        if (playerMovement != null)
            playerMovement.enabled = false;

        // Write night text
        Subtext.text = "";
        Holder = "Night 100";
        foreach (char c in Holder)
        {
            Subtext.text += c;
            yield return new WaitForSeconds(WriteSpeed);
        }

        // Wait a bit before changing scene
        yield return new WaitForSeconds(5f);

        // Load the next scene
        SceneManager.LoadScene(1);
        Debug.Log("Loaded the next scene");
    }
}