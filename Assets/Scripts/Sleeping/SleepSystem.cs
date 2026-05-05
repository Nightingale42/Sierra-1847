using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SleepSystem : MonoBehaviour
{
    [Header("Interaction")]
    public bool CanInteract = true;
    [SerializeField] private GameObject BlackPanel_GO;

    [Header("Movement Script")]
    public PlayerMovement playerMovement;

    [Header("UI (TMP for Night Text)")]
    public TextMeshProUGUI Subtext;  // TMP — for “Night 100”
    private string Holder;
    private float WriteSpeed = 0.5f;

    [Header("Interaction Text (Legacy)")]
    public Text InteractionText;  // Legacy UI — shared with Caminteract

    private void Start()
    {
        // Auto-find PlayerMovement if not assigned
        if (playerMovement == null)
        {
            playerMovement = FindObjectOfType<PlayerMovement>();
        }

        // Auto-find InteractionText if not assigned
        if (InteractionText == null)
        {
            GameObject found = GameObject.Find("InteractionText");
            if (found != null)
                InteractionText = found.GetComponent<Text>();

            if (InteractionText == null)
                Debug.LogError("SleepSystem could NOT find InteractionText in scene!");
        }

        // Subtext must also be found (TMP)
        if (Subtext == null)
        {
            GameObject foundTMP = GameObject.Find("Subtext");
            if (foundTMP != null)
                Subtext = foundTMP.GetComponent<TextMeshProUGUI>();

            if (Subtext == null)
                Debug.LogError("SleepSystem could NOT find TMP Subtext in scene!");
        }
    }

    private void Update()
    {
        if (!CanInteract)
            return;

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3f))
        {
            if (hit.collider.CompareTag("Bed"))
            {
                // Show “Press E to sleep”
                if (InteractionText != null)
                    InteractionText.text = "Press E to sleep";

                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(SleepCO());
                }

                return; // prevents clearing while still looking
            }
        }

        // Not looking at bed → clear text
        if (InteractionText != null)
            InteractionText.text = "";
    }

    IEnumerator SleepCO()
    {
        BlackPanel_GO.SetActive(true);
        CanInteract = false;

        yield return new WaitForSeconds(2f);

        if (playerMovement != null)
            playerMovement.enabled = false;

        // TMP text writing
        Subtext.text = "";
        Holder = "Vertical slice";

        foreach (char c in Holder)
        {
            Subtext.text += c;
            yield return new WaitForSeconds(WriteSpeed);
        }

        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene(2);
    }
}