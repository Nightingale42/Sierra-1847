using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
using UnityEngine.SceneManagement;

public class D2WagonCamInteract : MonoBehaviour
{
    // ================= INTERACTION =================
    [Header("Interaction")]
    public Transform RayOrigin;
    public Text InteractionText;
    public float InteractDistance = 2f;
    public bool CanInteract = true;

    // ================= CAMERAS =================
    [Header("Cameras")]
    public CinemachineVirtualCamera PlayerVcam;
    public CinemachineVirtualCamera TalkZoomVcam;
    public CinemachineVirtualCamera RedFriendZoomVcam;

    // ================= PLAYER =================
    [Header("Player")]
    public PlayerMovement FpsController;
    public LookAtFunction LookAtScript;

    // ================= DIALOGUE UI =================
    [Header("Dialogue UI")]
    public GameObject TalkPanel;
    public GameObject ChoicePack;
    public Text SubText;

    // ================= NIGHT TRANSITION =================
    [Header("Night Transition")]
    public GameObject BlackPanel_GO;
    public TextMeshProUGUI NightSubtext;
    public string NightText = "Night 100";
    public float FadeDelay = 2f;
    public float WriteSpeed = 0.5f;
    public int NextSceneIndex = 1;

    // ================= STATE =================
    private float typeSpeed = 0.04f;
    private bool endingDay;

    // ================= WOOD =================
    public static int count;

    // --------------------------------------------------
    void Update()
    {
        if (!CanInteract || RayOrigin == null)
        {
            ClearInteractionText();
            return;
        }

        Ray ray = new Ray(RayOrigin.position, RayOrigin.forward);

        if (!Physics.Raycast(ray, out RaycastHit hit, InteractDistance))
        {
            ClearInteractionText();
            return;
        }

        if (hit.collider.CompareTag("Friend"))
        {
            InteractionText.text = "Talk To Him";

            if (Input.GetKeyDown(KeyCode.E))
            {
                CanInteract = false;
                ClearInteractionText();
                StartCoroutine(TalkToFriendCO());
            }
        }
        else if (hit.collider.CompareTag("RedFriend"))
        {
            InteractionText.text = "Talk To Red Friend";

            if (Input.GetKeyDown(KeyCode.E))
            {
                CanInteract = false;
                ClearInteractionText();
                StartCoroutine(TalkToRedFriendCO());
            }
        }
        else
        {
            ClearInteractionText();
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
        yield return ExitDialogue(false);
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

        // -------- LOGS ACKNOWLEDGEMENT --------
        if (count > 2)
        {
            yield return TypeLine("Friend: ",
                "Thanks for collecting those logs. There's some soup in the communal pot. Don't ask what's in it.");

            yield return new WaitForSeconds(2f);
        }
        else
        {
            yield return TypeLine("Me: ",
                "Do you think it's starting to get cold aready?");
            yield return WaitForClick();

            yield return TypeLine("Friend: ",
                "It's a little cold, yeah. I'm sure it's just a cold front, we're heading into the mountains, eh?");
            yield return WaitForClick();

            yield return TypeLine("Me: ",
                "Yeah, the air is thinning a bit.");

            ChoicePack.SetActive(true);
            yield break;
        }

        yield return TypeLine("Friend: ",
            "Probably best to head to bed. We've got a big day ahead.");

        yield return new WaitForSeconds(2f);
        yield return ExitDialogue(true);
    }

    // ================= CHOICES =================
    public void Choice1Void() =>
        StartCoroutine(ChoiceCO("Me: ", "This is probably as cold as it'll get."));

    public void Choice2Void() =>
        StartCoroutine(ChoiceCO("Me: ", "Doesn't it snow up there? Seems like it's only going to get worse."));

    IEnumerator ChoiceCO(string speaker, string line)
    {
        ChoicePack.SetActive(false);

        // Player choice
        yield return TypeLine(speaker, line);
         yield return WaitForClick();

        // Friend response AFTER choice
        yield return TypeLine("Friend: ", "Yeah, you're probably right.");
        yield return WaitForClick();

         yield return TypeLine("Friend: ",
            "Looks like we're stopping ahead. Hopefully we can catch some meat for tonight.");
            yield return WaitForClick();

        yield return ExitDialogue(true);
    }

    // ================= EXIT =================
    IEnumerator ExitDialogue(bool endDay)
    {
        if (endingDay) yield break;
        endingDay = endDay;

        ClearInteractionText();

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
        CanInteract = !endDay;

        yield return new WaitForSeconds(0.5f);

        if (endDay)
            yield return StartCoroutine(NightTransitionCO());
    }

    // ================= NIGHT TRANSITION =================
    IEnumerator NightTransitionCO()
    {
        ClearInteractionText();
        BlackPanel_GO.SetActive(true);

        yield return new WaitForSeconds(FadeDelay);

        NightSubtext.text = "";

        foreach (char c in NightText)
        {
            NightSubtext.text += c;
            yield return new WaitForSeconds(WriteSpeed);
        }

        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(3);
    }

    // ================= HELPERS =================
    void EnterDialogue()
    {
        ClearInteractionText();

        FpsController.enabled = false;
        LookAtScript.IKActive = true;

        TalkZoomVcam.enabled = false;
        RedFriendZoomVcam.enabled = false;
    }

    void ClearInteractionText()
    {
        if (InteractionText != null)
            InteractionText.text = "";
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
}