using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class D3SleepSystem : MonoBehaviour
{
    [Header("Black Screen")]
    [SerializeField] private GameObject BlackPanel_GO;

    [Header("Movement Script")]
    public PlayerMovement playerMovement;

    [Header("UI (TMP for Night Text)")]
    public TextMeshProUGUI Subtext;
    private string Holder;
    private float WriteSpeed = 0.5f;

    private bool hasRun = false;

    private void Awake()
    {
        // Auto-find PlayerMovement if not assigned
        if (playerMovement == null)
            playerMovement = FindObjectOfType<PlayerMovement>();

        // Auto-find TMP Subtext if not assigned
        if (Subtext == null)
        {
            GameObject foundTMP = GameObject.Find("Subtext");
            if (foundTMP != null)
                Subtext = foundTMP.GetComponent<TextMeshProUGUI>();
        }
    }

    private void OnEnable()
    {
        // Prevent double-runs if object is toggled
        if (!hasRun)
        {
            hasRun = true;
            StartCoroutine(SleepCO());
        }
    }

    IEnumerator SleepCO()
    {
        if (BlackPanel_GO != null)
            BlackPanel_GO.SetActive(true);

        yield return new WaitForSeconds(2f);

        if (playerMovement != null)
            playerMovement.enabled = false;

        // TMP typewriter text
        if (Subtext != null)
        {
            Subtext.text = "";
            Holder = "Night 100";

            foreach (char c in Holder)
            {
                Subtext.text += c;
                yield return new WaitForSeconds(WriteSpeed);
            }
        }

        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene(6);
    }
}
