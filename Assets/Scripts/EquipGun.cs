using System.Collections;
using UnityEngine;
using TMPro;

public class EquipGun : MonoBehaviour
{
    [Header("Gun")]
    public GameObject Gun;
    public Transform WeaponParent;

    [Header("UI")]
    public TextMeshProUGUI InteractionText;
    public float FadeSpeed = 6f;
    public float DropTextDuration = 7f;

    private bool isEquipped;
    private bool playerInRange;

    private Coroutine fadeRoutine;
    private Coroutine dropTextTimer;

    void Start()
    {
        Gun.GetComponent<Rigidbody>().isKinematic = false;
        SetTextAlpha(0f);
    }

    void Update()
    {
        if (!playerInRange)
            return;

        if (!isEquipped && Input.GetKeyDown(KeyCode.E))
        {
            EquipWeapon();
        }

        if (isEquipped && Input.GetKeyDown(KeyCode.F))
        {
            Drop();
        }
    }

    // ───────── TRIGGERS ─────────

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;

        if (!isEquipped)
            ShowText("Press E to equip");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        HideText();
    }

    // ───────── LOGIC ─────────

    void EquipWeapon()
    {
        Gun.GetComponent<Rigidbody>().isKinematic = true;
        Gun.GetComponent<MeshCollider>().enabled = false;

        Gun.transform.SetParent(WeaponParent);
        Gun.transform.localPosition = Vector3.zero;
        Gun.transform.localRotation = Quaternion.identity;

        isEquipped = true;

        ShowText("Press F to drop");

        if (dropTextTimer != null)
            StopCoroutine(dropTextTimer);

        dropTextTimer = StartCoroutine(HideDropTextAfterDelay());
    }

    void Drop()
    {
        Gun.transform.SetParent(null);

        Gun.GetComponent<Rigidbody>().isKinematic = false;
        Gun.GetComponent<MeshCollider>().enabled = true;

        isEquipped = false;

        HideText();
    }

    // ───────── UI ─────────

    void ShowText(string message)
    {
        if (InteractionText == null) return;

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

        if (isEquipped)
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
        if (InteractionText == null) return;

        Color c = InteractionText.color;
        c.a = alpha;
        InteractionText.color = c;
    }
}
