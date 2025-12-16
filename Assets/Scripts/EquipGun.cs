using System.Collections;
using UnityEngine;
using TMPro;

public class EquipGun : MonoBehaviour
{
    [Header("Gun")]
    public GameObject Gun;
    public Transform WeaponParent;

    [Header("UI")]
    public TextMeshProUGUI InteractionText; // Assign in Inspector
    public float FadeSpeed = 6f;
    public float DropTextDuration = 7f;

    private bool isEquipped = false;
    private bool playerInRange = false;

    private Coroutine fadeRoutine;
    private Coroutine dropTextTimer;

    void Start()
    {
        Gun.GetComponent<Rigidbody>().isKinematic = true;
        SetTextAlpha(0f);
    }

    void Update()
    {
        if (isEquipped && Input.GetKeyDown(KeyCode.F))
        {
            Drop();
        }
    }

    void EquipWeapon()
    {
        Gun.GetComponent<Rigidbody>().isKinematic = true;
        Gun.GetComponent<MeshCollider>().enabled = false;

        Gun.transform.position = WeaponParent.position;
        Gun.transform.rotation = WeaponParent.rotation;
        Gun.transform.SetParent(WeaponParent);

        isEquipped = true;

        ShowText("Press F to drop");

        if (dropTextTimer != null)
            StopCoroutine(dropTextTimer);

        dropTextTimer = StartCoroutine(HideDropTextAfterDelay());
    }

    void Drop()
    {
        WeaponParent.DetachChildren();

        Gun.transform.eulerAngles = new Vector3(Gun.transform.position.x, Gun.transform.position.z, Gun.transform.position.y);
        Gun.GetComponent<Rigidbody>().isKinematic = false;
        Gun.GetComponent<MeshCollider>().enabled = true;

        isEquipped = false;
        playerInRange = false;

        if (dropTextTimer != null)
            StopCoroutine(dropTextTimer);

        HideText();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKey(KeyCode.E))
        {
            EquipWeapon();
        }
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
        if (InteractionText == null) return;

        Color c = InteractionText.color;
        c.a = alpha;
        InteractionText.color = c;
    }
}