using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class Caminteract : MonoBehaviour
{
    // Interaction
    public bool TalkToActualFriend = false;
    public bool TalkToRedFriend = false;
    public bool TalkToBlueFriend = false;
    public bool TalkToPinkFriend = false;
    public bool TalkToOrangeFriend = false;
    public bool TalkToYellowFriend = false;
    public bool TalkToGreenFriend = false;
    public bool TalkToPurpleFriend = false;
    public LookAt2 LookAtScript;
    public Text InteractionText;
    private float InteractDistance = 2f;
    public bool CanInteract = true;

    // Wood collecting
    public static int count;

    // Cameras & Movement
    public CinemachineVirtualCamera PlayerVcam;
    public CinemachineVirtualCamera TalkZoomVcam;
    public CinemachineVirtualCamera RedFriendZoomVcam;
    public CinemachineVirtualCamera BlueFriendZoomVcam;
    public CinemachineVirtualCamera PinkFriendZoomVcam;
    public CinemachineVirtualCamera OrangeFriendZoomVcam;
     public CinemachineVirtualCamera YellowFriendZoomVcam;
      public CinemachineVirtualCamera GreenFriendZoomVcam;
      public CinemachineVirtualCamera PurpleFriendZoomVcam;
    public NewPlayerMovement FpsController;

    // Dialogue UI
    public GameObject TalkPanel;
    public GameObject ChoicePack;
    public Text SubText;

    string holder;
    float time = 0.05f;

    // Sleep System
    private SleepSystem sleepSystem;
    private bool sleepEnabled = false;

    // ------------------------------------------
    void Start()
    {
        sleepSystem = GetComponent<SleepSystem>();  // Find SleepSystem on same GameObject
    }

    // ------------------------------------------
    // UPDATE – Interaction Raycast
    // ------------------------------------------
  void Update()
{
    if (!CanInteract)
        return;

    Ray ray1 = new Ray(transform.position, transform.forward);
    RaycastHit hit1;

    if (Physics.Raycast(ray1, out hit1, InteractDistance))
    {
        if (hit1.collider.CompareTag("Friend"))
        {
            InteractionText.text = "Talk To Him";

            if (Input.GetKeyDown(KeyCode.E))
            {
                CanInteract = false;
                StartCoroutine(TalkToFriendCO());
            }
        }
        else if (hit1.collider.CompareTag("RedFriend"))
        {
            InteractionText.text = "Talk to Red Friend";

            if (Input.GetKeyDown(KeyCode.E))
            {
                CanInteract = false;
                StartCoroutine(TalkToRedFriendCO());
            }
        }
        else if (hit1.collider.CompareTag("BlueFriend"))   // NEW OPTION
        {
            InteractionText.text = "Talk to Blue Friend";

            if (Input.GetKeyDown(KeyCode.E))
            {
                CanInteract = false;
                StartCoroutine(TalkToBlueFriendCO());
            }
        }
         else if (hit1.collider.CompareTag("PinkFriend"))   // NEW OPTION
        {
            InteractionText.text = "Talk to Pink Friend";

            if (Input.GetKeyDown(KeyCode.E))
            {
                CanInteract = false;
                StartCoroutine(TalkToPinkFriendCO());
            }
        }
         else if (hit1.collider.CompareTag("OrangeFriend"))   // NEW OPTION
        {
            InteractionText.text = "Talk to Orange Friend";

            if (Input.GetKeyDown(KeyCode.E))
            {
                CanInteract = false;
                StartCoroutine(TalkToOrangeFriendCO());
            }
        }
        else if (hit1.collider.CompareTag("YellowFriend"))   // NEW OPTION
        {
            InteractionText.text = "Talk to Yellow Friend";

            if (Input.GetKeyDown(KeyCode.E))
            {
                CanInteract = false;
                StartCoroutine(TalkToYellowFriendCO());
            }
        }
          else if (hit1.collider.CompareTag("GreenFriend"))   // NEW OPTION
        {
            InteractionText.text = "Talk to Green Friend";

            if (Input.GetKeyDown(KeyCode.E))
            {
                CanInteract = false;
                StartCoroutine(TalkToGreenFriendCO());
            }
        }

         else if (hit1.collider.CompareTag("PurpleFriend"))   // NEW OPTION
        {
            InteractionText.text = "Talk to Purple Friend";

            if (Input.GetKeyDown(KeyCode.E))
            {
                CanInteract = false;
                StartCoroutine(TalkToPurpleFriendCO());
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
    // RED FRIEND DIALOGUE
    // ------------------------------------------
    IEnumerator TalkToRedFriendCO()
    {
        TalkToRedFriend = true;
        InteractionText.text = "";
        FpsController.enabled = false;
        
        PurpleFriendZoomVcam.enabled = false;
        OrangeFriendZoomVcam.enabled = false;
        YellowFriendZoomVcam.enabled = false;
        GreenFriendZoomVcam.enabled = false;
        PinkFriendZoomVcam.enabled = false;
        RedFriendZoomVcam.enabled = true;
        BlueFriendZoomVcam.enabled = false;
        PlayerVcam.enabled = false;
        TalkZoomVcam.enabled = false;

        LookAtScript.IKActive = true;

        yield return new WaitForSeconds(1f);
        TalkPanel.SetActive(true);

        // Dialogue 1
        SubText.text = "Me: ";
        holder = "How're you holding up kid?";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();

        // Dialogue 2
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
    // BLUE FRIEND DIALOGUE
    // ------------------------------------------

 IEnumerator TalkToBlueFriendCO()
    {
        TalkToBlueFriend = true;
        InteractionText.text = "";
        FpsController.enabled = false;

        PurpleFriendZoomVcam.enabled = false;
        OrangeFriendZoomVcam.enabled = false;
        YellowFriendZoomVcam.enabled = false;
        GreenFriendZoomVcam.enabled = false;
        PinkFriendZoomVcam.enabled = false;
        BlueFriendZoomVcam.enabled = true;
        RedFriendZoomVcam.enabled = false;
        PlayerVcam.enabled = false;
        TalkZoomVcam.enabled = false;

        LookAtScript.IKActive = true;

        yield return new WaitForSeconds(1f);
        TalkPanel.SetActive(true);

        // Dialogue 1
        SubText.text = "Me: ";
        holder = "you are blue?";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();

        // Dialogue 2
        SubText.text = "Kid: ";
        holder = "for now yes.";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();

        StartCoroutine(FinalCO());
    }
// ------------------------------------------
    // PINK FRIEND DIALOGUE
    // ------------------------------------------

IEnumerator TalkToPinkFriendCO()
    {
        TalkToPinkFriend = true;
        InteractionText.text = "";
        FpsController.enabled = false;

        PurpleFriendZoomVcam.enabled = false;
        PinkFriendZoomVcam.enabled = true;
        OrangeFriendZoomVcam.enabled = false;
        YellowFriendZoomVcam.enabled = false;
        GreenFriendZoomVcam.enabled = false;
        BlueFriendZoomVcam.enabled = false;
        RedFriendZoomVcam.enabled = false;
        PlayerVcam.enabled = false;
        TalkZoomVcam.enabled = false;

        LookAtScript.IKActive = true;

        yield return new WaitForSeconds(1f);
        TalkPanel.SetActive(true);

        // Dialogue 1
        SubText.text = "Me: ";
        holder = "you are pink?";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();

        // Dialogue 2
        SubText.text = "Kid: ";
        holder = "for now yes.";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();

        StartCoroutine(FinalCO());
    }

// ------------------------------------------
    // ORANGE FRIEND DIALOGUE
    // ------------------------------------------


IEnumerator TalkToOrangeFriendCO()
    {
        TalkToOrangeFriend = true;
        InteractionText.text = "";
        FpsController.enabled = false;

        PurpleFriendZoomVcam.enabled = false;
        OrangeFriendZoomVcam.enabled = true;
        YellowFriendZoomVcam.enabled = false;
        GreenFriendZoomVcam.enabled = false;
        PinkFriendZoomVcam.enabled = false;
        BlueFriendZoomVcam.enabled = false;
        RedFriendZoomVcam.enabled = false;
        PlayerVcam.enabled = false;
        TalkZoomVcam.enabled = false;

        LookAtScript.IKActive = true;

        yield return new WaitForSeconds(1f);
        TalkPanel.SetActive(true);

        // Dialogue 1
        SubText.text = "Me: ";
        holder = "you are orange?";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();

        // Dialogue 2
        SubText.text = "Kid: ";
        holder = "for now yes.";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();

        StartCoroutine(FinalCO());
    }

    
// ------------------------------------------
    // YELLOW FRIEND DIALOGUE
    // ------------------------------------------


IEnumerator TalkToYellowFriendCO()
    {
        TalkToYellowFriend = true;
        InteractionText.text = "";
        FpsController.enabled = false;

        PurpleFriendZoomVcam.enabled = false;
        OrangeFriendZoomVcam.enabled = false;
        YellowFriendZoomVcam.enabled = true;
        GreenFriendZoomVcam.enabled = false;
        PinkFriendZoomVcam.enabled = false;
        BlueFriendZoomVcam.enabled = false;
        RedFriendZoomVcam.enabled = false;
        PlayerVcam.enabled = false;
        TalkZoomVcam.enabled = false;

        LookAtScript.IKActive = true;

        yield return new WaitForSeconds(1f);
        TalkPanel.SetActive(true);

        // Dialogue 1
        SubText.text = "Me: ";
        holder = "you are yellow?";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();

        // Dialogue 2
        SubText.text = "Kid: ";
        holder = "for now yes.";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();

        StartCoroutine(FinalCO());
    }

// ------------------------------------------
    // GREEN FRIEND DIALOGUE
    // ------------------------------------------

IEnumerator TalkToGreenFriendCO()
    {
        TalkToGreenFriend = true;
        InteractionText.text = "";
        FpsController.enabled = false;

        PurpleFriendZoomVcam.enabled = false;
        GreenFriendZoomVcam.enabled = true;
        OrangeFriendZoomVcam.enabled = false;
        YellowFriendZoomVcam.enabled = false;
        PinkFriendZoomVcam.enabled = false;
        BlueFriendZoomVcam.enabled = false;
        RedFriendZoomVcam.enabled = false;
        PlayerVcam.enabled = false;
        TalkZoomVcam.enabled = false;

        LookAtScript.IKActive = true;

        yield return new WaitForSeconds(1f);
        TalkPanel.SetActive(true);

        // Dialogue 1
        SubText.text = "Me: ";
        holder = "you are green?";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();

        // Dialogue 2
        SubText.text = "Kid: ";
        holder = "for now yes.";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();

        StartCoroutine(FinalCO());
    }


// ------------------------------------------
    // PURPLE FRIEND DIALOGUE
    // ------------------------------------------

IEnumerator TalkToPurpleFriendCO()
    {
        TalkToPurpleFriend = true;
        InteractionText.text = "";
        FpsController.enabled = false;

        GreenFriendZoomVcam.enabled = false;
        OrangeFriendZoomVcam.enabled = false;
        PurpleFriendZoomVcam.enabled = true;
        YellowFriendZoomVcam.enabled = false;
        PinkFriendZoomVcam.enabled = false;
        BlueFriendZoomVcam.enabled = false;
        RedFriendZoomVcam.enabled = false;
        PlayerVcam.enabled = false;
        TalkZoomVcam.enabled = false;

        LookAtScript.IKActive = true;

        yield return new WaitForSeconds(1f);
        TalkPanel.SetActive(true);

        // Dialogue 1
        SubText.text = "Me: ";
        holder = "you are purple?";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();

        // Dialogue 2
        SubText.text = "Kid: ";
        holder = "for now yes.";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();

        StartCoroutine(FinalCO());
    }

    // ------------------------------------------
    // MAIN FRIEND DIALOGUE (with priority system)
    // ------------------------------------------

    IEnumerator TalkToFriendCO()
    {
        TalkToActualFriend = true;
        InteractionText.text = "";
        FpsController.enabled = false;

        TalkZoomVcam.enabled = true;
        PlayerVcam.enabled = false;
        RedFriendZoomVcam.enabled = false;
        BlueFriendZoomVcam.enabled = false;
        PinkFriendZoomVcam.enabled = false;
        OrangeFriendZoomVcam.enabled = false;
        YellowFriendZoomVcam.enabled = false;
        GreenFriendZoomVcam.enabled = false;
        PurpleFriendZoomVcam.enabled = false;

        LookAtScript.IKActive = true;

        yield return new WaitForSeconds(1f);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        TalkPanel.SetActive(true);

        Debug.Log("TalkToFriendCO started. count = " + count);
        Debug.Log("TalkToFriendCO started. SliceEaten = " + EatingSystem.SliceEaten);

        // ==================================================
        // PRIORITY SYSTEM
        // 1. Soup Dialogue (also activates SleepSystem)
        // 2. Logs Dialogue
        // 3. Default Dialogue
        // ==================================================

        // -----------------------------
        // 1. SOUP PRIORITY
        // -----------------------------
        if (EatingSystem.SliceEaten > 2)
        {
            Debug.Log("PRIORITY: Soup dialogue triggered.");

            // Enable SleepSystem ONCE
            if (!sleepEnabled)
            {
                sleepEnabled = true;

                if (sleepSystem != null)
                {
                    sleepSystem.enabled = true;
                    Debug.Log("SleepSystem ENABLED because SliceEaten > 2");
                }
                else
                {
                    Debug.LogError("SleepSystem NOT FOUND on this GameObject!");
                }
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

        // -----------------------------
        // 2. LOGS PRIORITY
        // -----------------------------
        if (count > 2)
        {
            Debug.Log("PRIORITY: Logs dialogue triggered.");

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

        // -----------------------------
        // 3. DEFAULT DIALOGUE
        // -----------------------------
        Debug.Log("PRIORITY: Default dialogue triggered.");

        SubText.text = "Me: ";
        holder = "Hey man, can I help with anything?";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();

        SubText.text = "Friend: ";
        holder = "Hey there. We could use some help collecting wood for the fire tonight.";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();

        SubText.text = "Friend: ";
        holder = "The Axe is over by the barrels. Oh- and try to only chop the dead trees. They burn easier.";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return new WaitForSeconds(1f);
        ChoicePack.SetActive(true);
    }

    // ------------------------------------------
    // CHOICE BUTTONS
    // ------------------------------------------
    public void Choice1Void() => StartCoroutine(Choice1CO());
    public void Choice2Void() => StartCoroutine(Choice2CO());

    IEnumerator Choice1CO()
    {
        ChoicePack.SetActive(false);

        SubText.text = "Me: ";
        holder = "Yeah I can help out with that.";
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
        holder = "No, sorry I'm busy at the moment.";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return new WaitForSeconds(2f);
        StartCoroutine(FinalCO());
    }

    // ------------------------------------------
    // RESET AFTER ANY DIALOGUE
    // ------------------------------------------
    IEnumerator FinalCO()
    {
        TalkToRedFriend = false;
        TalkToActualFriend = false;
        TalkToPinkFriend = false;
        TalkToOrangeFriend = false;
        TalkToYellowFriend = false;
        TalkToBlueFriend = false;
        TalkToGreenFriend = false;
        TalkToPurpleFriend = false;
        

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

    // ------------------------------------------
    // WAIT FOR MOUSE CLICK
    // ------------------------------------------
    IEnumerator MousePress()
    {
        Debug.Log("Waiting for mouse press...");
        while (!Input.GetMouseButtonDown(0))
            yield return null;
        Debug.Log("Mouse pressed!");
    }
}