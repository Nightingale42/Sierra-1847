using UnityEngine;
using TMPro;
using System.Collections;

public class EquipAxe : MonoBehaviour
{
    public GameObject Axe;
    public Transform AxeParent;
    public TextMeshProUGUI InteractionText;

    public float FadeSpeed = 6f;
    public float DropTextDuration = 7f;

    private bool isEquipped = false;
    private bool playerInRange = false;

    private Coroutine fadeRoutine;
    private Coroutine dropTextTimer;

    void Start()
    {
        Axe.GetComponent<Rigidbody>().isKinematic = true;
        SetTextAlpha(0f);
    }

    void Update()
    {
        if (playerInRange && !isEquipped && Input.GetKeyDown(KeyCode.E))
        {
            EquipAxePlayer();
        }

        if (isEquipped && Input.GetKeyDown(KeyCode.F))
        {
            Drop();
        }
    }

    void EquipAxePlayer()
    {
        Axe.GetComponent<Rigidbody>().isKinematic = true;
        Axe.GetComponent<MeshCollider>().enabled = false;

        Axe.transform.SetParent(AxeParent);
        Axe.transform.position = AxeParent.position;
        Axe.transform.rotation = AxeParent.rotation;

        isEquipped = true;

        ShowText("Press F to drop");

        if (dropTextTimer != null)
            StopCoroutine(dropTextTimer);

        dropTextTimer = StartCoroutine(HideDropTextAfterDelay());
    }

    void Drop()
    {
        AxeParent.DetachChildren();

        Axe.GetComponent<Rigidbody>().isKinematic = false;
        Axe.GetComponent<MeshCollider>().enabled = true;

        isEquipped = false;
        playerInRange = false;

        if (dropTextTimer != null)
            StopCoroutine(dropTextTimer);

        HideText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isEquipped)
        {
            playerInRange = true;
            ShowText("Press E to equip");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !isEquipped)
        {
            playerInRange = false;
            HideText();
        }
    }

    void ShowText(string message)
    {
        InteractionText.text = message;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeTo(1f));
    }

    void HideText()
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeTo(0f));
    }

    IEnumerator HideDropTextAfterDelay()
    {
        yield return new WaitForSeconds(DropTextDuration);

        if (isEquipped) // only fade if still equipped
            HideText();
    }

    IEnumerator FadeTo(float targetAlpha)
    {
        Color c = InteractionText.color;

        while (!Mathf.Approximately(c.a, targetAlpha))
        {
            c.a = Mathf.MoveTowards(c.a, targetAlpha, Time.deltaTime * FadeSpeed);
            InteractionText.color = c;
            yield return null;
        }
    }

    void SetTextAlpha(float alpha)
    {
        Color c = InteractionText.color;
        c.a = alpha;
        InteractionText.color = c;
    }
}