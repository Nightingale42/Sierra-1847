using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class D2Caminteract : MonoBehaviour
{
    // ---------------- INTERACTION ----------------
    public bool TalkToActualFriend = false;
    public bool TalkToRedFriend = false;
    public LookAt3 LookAtScript;
    public Text InteractionText;
    private float InteractDistance = 2f;
    public bool CanInteract = true;

    // ---------------- PROGRESSION ----------------
    public static int count;

    // ---------------- CAMERAS ----------------
    public CinemachineVirtualCamera PlayerVcam;
    public CinemachineVirtualCamera TalkZoomVcam;
    public CinemachineVirtualCamera RedFriendZoomVcam;
    public NewPlayerMovement FpsController;

    // ---------------- UI ----------------
    public GameObject TalkPanel;
    public GameObject ChoicePack;
    public Text SubText;

    string holder;
    float time = 0.05f;

    // ---------------- SLEEP SYSTEM ----------------
    private SleepSystem sleepSystem;
    private bool sleepEnabled = false;

    void Start()
    {
        sleepSystem = GetComponent<SleepSystem>();
    }

    // ------------------------------------------
    // UPDATE
    // ------------------------------------------
    void Update()
    {
        if (!CanInteract)
            return;

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, InteractDistance))
        {
            if (hit.collider.CompareTag("Friend"))
            {
                InteractionText.text = "Talk To Him";

                if (Input.GetKeyDown(KeyCode.E))
                {
                    CanInteract = false;
                    StartCoroutine(TalkToFriendCO());
                }
            }
            else if (hit.collider.CompareTag("RedFriend"))
            {
                InteractionText.text = "Talk to Red Friend";

                if (Input.GetKeyDown(KeyCode.E))
                {
                    CanInteract = false;
                    StartCoroutine(TalkToRedFriendCO());
                }
            }
            else
            {
                InteractionText.text = "";
            }
        }
        else
        {
            InteractionText.text = "";
        }
    }

    // ------------------------------------------
    // RED FRIEND
    // ------------------------------------------
    IEnumerator TalkToRedFriendCO()
    {
        TalkToRedFriend = true;
        InteractionText.text = "";
        FpsController.enabled = false;

        RedFriendZoomVcam.enabled = true;
        PlayerVcam.enabled = false;
        TalkZoomVcam.enabled = false;

        LookAtScript.IKActive = true;

        yield return new WaitForSeconds(1f);

        TalkPanel.SetActive(true);

        SubText.text = "Me: ";
        holder = "How're you holding up kid?";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();

        SubText.text = "Kid: ";
        holder = "Not great I guess... It's so cold out and my stomach hurts. I haven't eaten in days.";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();

        StartCoroutine(FinalCO());
    }

    // ------------------------------------------
    // MAIN FRIEND DIALOGUE
    // ------------------------------------------
    IEnumerator TalkToFriendCO()
    {
        TalkToActualFriend = true;
        InteractionText.text = "";
        FpsController.enabled = false;

        TalkZoomVcam.enabled = true;
        PlayerVcam.enabled = false;
        RedFriendZoomVcam.enabled = false;

        LookAtScript.IKActive = true;

        yield return new WaitForSeconds(1f);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        TalkPanel.SetActive(true);

        // ---------- PRIORITY CHECKS ----------

        if (EatingSystem.SliceEaten > 2)
        {
            if (!sleepEnabled)
            {
                sleepEnabled = true;
                if (sleepSystem != null)
                    sleepSystem.enabled = true;
            }

            SubText.text = "Friend: ";
            holder = "Looks like you got some dinner. Probably best to head to bed, we've got a big day ahead.";
            foreach (char c in holder)
            {
                SubText.text += c;
                yield return new WaitForSeconds(time);
            }

            yield return new WaitForSeconds(2f);
            StartCoroutine(FinalCO());
            yield break;
        }

        if (count > 2)
        {
            SubText.text = "Friend: ";
            holder = "Thanks for collecting those logs. There's some soup in the communal pot. Don't ask what's in it.";
            foreach (char c in holder)
            {
                SubText.text += c;
                yield return new WaitForSeconds(time);
            }

            yield return new WaitForSeconds(2f);
            StartCoroutine(FinalCO());
            yield break;
        }

        // ---------- DEFAULT DIALOGUE ----------
        SubText.text = "Friend: ";
        holder = "I'm real hungry. Damn Marison keeping all the food to himself.";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();

        SubText.text = "Me: ";
        holder = "You reckon I could scrounge up something? Theres got to be some edible stuff in these woods.";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();

        SubText.text = "Friend: ";
        holder = "You think so? Might just be worth it. Could be some wild carrots or something out there, i'd even eat a wild mushroom right about now.";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return new WaitForSeconds(1f);
        ChoicePack.SetActive(true);
    }

    // ------------------------------------------
    // CHOICES
    // ------------------------------------------
    public void Choice1Void() => StartCoroutine(Choice1CO());
    public void Choice2Void() => StartCoroutine(Choice2CO());

    IEnumerator Choice1CO()
    {
        ChoicePack.SetActive(false);

        SubText.text = "Me: ";
        holder = "Dude you're making me hungry... i'm in.";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return new WaitForSeconds(1f);

        SubText.text = "Friend: ";
        holder = "haha, you're easy. Let me know if you find anything.";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return new WaitForSeconds(2f);
        StartCoroutine(FinalCO());
    }

    IEnumerator Choice2CO()
    {
        ChoicePack.SetActive(false);

        SubText.text = "Me: ";
        holder = "That sounds like a terrible idea.";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return new WaitForSeconds(2f);
        StartCoroutine(FinalCO());
    }

    // ------------------------------------------
    // RESET
    // ------------------------------------------
    IEnumerator FinalCO()
    {
        TalkToRedFriend = false;
        TalkToActualFriend = false;

        TalkPanel.SetActive(false);
        ChoicePack.SetActive(false);
        SubText.text = "";

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        LookAtScript.IKActive = false;

        PlayerVcam.enabled = true;
        TalkZoomVcam.enabled = false;
        RedFriendZoomVcam.enabled = false;

        FpsController.enabled = true;
        CanInteract = true;

        yield return null;
    }

    IEnumerator MousePress()
    {
        while (!Input.GetMouseButtonDown(0))
            yield return null;
    }
}