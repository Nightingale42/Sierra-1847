using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class WagonCamInteract : MonoBehaviour
{
    // ---------------- INTERACTION ----------------
    [Header("Interaction")]
    public Transform RayOrigin;
    public Text InteractionText;
    public float InteractDistance = 2f;
    public bool CanInteract = true;

    // ---------------- STATE ----------------
    public bool TalkToActualFriend;
    public bool TalkToRedFriend;

    // ---------------- CAMERAS ----------------
    [Header("Cameras")]
    public CinemachineVirtualCamera PlayerVcam;
    public CinemachineVirtualCamera TalkZoomVcam;
    public CinemachineVirtualCamera RedFriendZoomVcam;

    // ---------------- PLAYER ----------------
    [Header("Player")]
    public PlayerMovement FpsController;
    public LookAtFunction LookAtScript;

    // ---------------- UI ----------------
    [Header("Dialogue UI")]
    public GameObject TalkPanel;
    public GameObject ChoicePack;
    public Text SubText;

    // ---------------- SYSTEMS ----------------
    private SleepSystem sleepSystem;
    private bool sleepEnabled;

    // ---------------- DIALOGUE ----------------
    private string holder;
    private float typeSpeed = 0.05f;

    // ---------------- WOOD ----------------
    public static int count;

    // --------------------------------------------------
    void Awake()
    {
        sleepSystem = GetComponent<SleepSystem>();
    }

    // --------------------------------------------------
    void Update()
    {
        if (!CanInteract || RayOrigin == null)
        {
            InteractionText.text = "";
            return;
        }

        Ray ray = new Ray(RayOrigin.position, RayOrigin.forward);
        Debug.DrawRay(ray.origin, ray.direction * InteractDistance, Color.green);

        if (!Physics.Raycast(ray, out RaycastHit hit, InteractDistance))
        {
            InteractionText.text = "";
            return;
        }

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
            InteractionText.text = "Talk To Red Friend";

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

    // ================= RED FRIEND =================
    IEnumerator TalkToRedFriendCO()
    {
        EnterDialogue();

        RedFriendZoomVcam.enabled = true;
        PlayerVcam.enabled = false;

        yield return new WaitForSeconds(1f);
        TalkPanel.SetActive(true);

        yield return TypeLine("Me: ", "How're you holding up kid?");
        yield return WaitForClick();

        yield return TypeLine("Kid: ",
            "Not great I guess... It's so cold out and my stomach hurts. I haven't eaten in days.");

        yield return WaitForClick();
        yield return ExitDialogue();
    }

    // ================= MAIN FRIEND =================
    IEnumerator TalkToFriendCO()
    {
        EnterDialogue();

        TalkZoomVcam.enabled = true;
        PlayerVcam.enabled = false;

        yield return new WaitForSeconds(1f);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        TalkPanel.SetActive(true);

        // -------- PRIORITY 1: SOUP --------
        if (EatingSystem.SliceEaten > 2)
        {
            EnableSleepSystem();

            yield return TypeLine("Friend: ",
                "Looks like you got some dinner. Probably best to head to bed, we've got a big day ahead.");

            yield return new WaitForSeconds(2f);
            yield return ExitDialogue();
            yield break;
        }

        // -------- PRIORITY 2: LOGS --------
        if (count > 2)
        {
            yield return TypeLine("Friend: ",
                "Thanks for collecting those logs. There's some soup in the communal pot. Don't ask what's in it.");

            yield return new WaitForSeconds(2f);
            yield return ExitDialogue();
            yield break;
        }

        // -------- DEFAULT --------
        yield return TypeLine("Me: ", "Hey man, can I help with anything?");
        yield return WaitForClick();

        yield return TypeLine("Friend: ",
            "Hey there. We could use some help collecting wood for the cabins.");
        yield return WaitForClick();

        yield return TypeLine("Friend: ",
            "The Axe is over by the barrels. Are you in?");

        ChoicePack.SetActive(true);
    }

    // ================= CHOICES =================
    public void Choice1Void() => StartCoroutine(ChoiceCO(
        "Me: ", "Yeah I can help out with that."
    ));

    public void Choice2Void() => StartCoroutine(ChoiceCO(
        "Me: ", "No, sorry I'm busy at the moment."
    ));

    IEnumerator ChoiceCO(string speaker, string line)
    {
        ChoicePack.SetActive(false);
        yield return TypeLine(speaker, line);
        yield return new WaitForSeconds(2f);
        yield return ExitDialogue();
    }

    // ================= HELPERS =================
    void EnterDialogue()
    {
        TalkToActualFriend = true;
        InteractionText.text = "";

        FpsController.enabled = false;
        LookAtScript.IKActive = true;

        TalkZoomVcam.enabled = false;
        RedFriendZoomVcam.enabled = false;
    }

    IEnumerator ExitDialogue()
    {
        TalkToActualFriend = false;
        TalkToRedFriend = false;

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

    IEnumerator TypeLine(string speaker, string line)
    {
        SubText.text = speaker;

        foreach (char c in line)
        {
            SubText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    IEnumerator WaitForClick()
    {
        while (!Input.GetMouseButtonDown(0))
            yield return null;
    }

    void EnableSleepSystem()
    {
        if (sleepEnabled || sleepSystem == null) return;

        sleepEnabled = true;
        sleepSystem.enabled = true;
    }
}
