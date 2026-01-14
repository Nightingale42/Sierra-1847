using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class D3WagonCamInteract : MonoBehaviour
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

    // ================= AUDIO =================
    [Header("Audio")]
    public AudioSource AudioSource;
    public AudioClip Crash;
    public AudioClip Horse;

    // ================= STATE =================
    private float typeSpeed = 0.05f;
    private bool endingDay;

    // ================= WOOD =================
    public static int count;

    // ================= CARRIAGE =================
    [Header("Carriage Object")]
    public GameObject CarriageObject;

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

    // ================= REMOVE NAVMESHAGENT =================
    void RemoveNavMeshAgent()
    {
        if (CarriageObject == null) return;

        NavMeshAgent agent = CarriageObject.GetComponent<NavMeshAgent>();
        if (agent == null) return;

        if (AudioSource != null)
        {
            if (Crash != null) AudioSource.PlayOneShot(Crash);
            if (Horse != null) AudioSource.PlayOneShot(Horse);
        }

        Destroy(agent);
        StartCoroutine(ForcedFriendDialogueCO());
    }

    // ================= FORCED FRIEND DIALOGUE =================
    IEnumerator ForcedFriendDialogueCO()
    {
        yield return new WaitForSeconds(5f);

        CanInteract = false;
        FpsController.enabled = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        LookAtScript.IKActive = true;

        PlayerVcam.enabled = false;
        TalkZoomVcam.enabled = true;

        yield return new WaitForSeconds(0.5f);
        TalkPanel.SetActive(true);

        yield return TypeLine("Friend: ", "What was that?");
        yield return WaitForClick();

        yield return TypeLine("Me: ", "It kind of felt like the horse got struck by lightning");
        yield return WaitForClick();

        yield return TypeLine("Friend: ", "Let's get out and check.");
        yield return WaitForClick();

        // 🔧 CLEAR DIALOGUE BEFORE TRANSITION
        SubText.text = "";
        TalkPanel.SetActive(false);

        yield return StartCoroutine(NightTransitionCO());

        yield return ExitDialogue(false);
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

        yield return TypeLine(
            "Kid: ",
            "Not great I guess... It's so cold out and my stomach hurts. I haven't eaten in days."
        );
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

        if (count > 2)
        {
            yield return TypeLine(
                "Friend: ",
                "Thanks for collecting those logs. There's some soup in the communal pot. Don't ask what's in it."
            );
            yield return new WaitForSeconds(2f);
        }
        else
        {
            yield return TypeLine("Me: ", "...It's snowing behind you.");
            yield return WaitForClick();

            yield return TypeLine("Friend: ", "You're kidding");
            yield return WaitForClick();

            yield return TypeLine(
                "Me: ",
                "Nope. I have a feeling we won't be finding any more carrots."
            );

            ChoicePack.SetActive(true);
            yield break;
        }

        yield return TypeLine(
            "Friend: ",
            "Probably best to head to bed. We've got a big day ahead."
        );
        yield return new WaitForSeconds(1f);

        yield return ExitDialogue(true);
    }

    // ================= CHOICES =================
    public void Choice1Void() =>
        StartCoroutine(ChoiceCO("Me: ", "Dude. We're screwed."));

    public void Choice2Void() =>
        StartCoroutine(ChoiceCO("Me: ", "As long as it doesn't get too high we should be fine."));

    IEnumerator ChoiceCO(string speaker, string line)
    {
        ChoicePack.SetActive(false);

        yield return TypeLine(speaker, line);
        yield return WaitForClick();

        yield return TypeLine("Friend: ", "Yeah man... Thats the spirit!");
        yield return WaitForClick();

        yield return TypeLine(
            "Friend: ",
            "I mean... you're from Ohio right? Plenty of snow there, shouldn't be anything new."
        );
        yield return WaitForClick();

        RemoveNavMeshAgent();
        yield return ExitDialogue(true);
    }

    // ================= NIGHT TRANSITION =================
    IEnumerator NightTransitionCO()
    {
        if (BlackPanel_GO != null)
            BlackPanel_GO.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        if (NightSubtext != null)
        {
            NightSubtext.text = "";

            foreach (char c in NightText)
            {
                NightSubtext.text += c;
                yield return new WaitForSeconds(typeSpeed);
            }
        }

        yield return new WaitForSeconds(FadeDelay);
        SceneManager.LoadScene(5);
    }

    // ================= EXIT =================
    IEnumerator ExitDialogue(bool endDay)
    {
        if (endingDay) yield break;
        endingDay = endDay;

        ClearInteractionText();
        ChoicePack.SetActive(false);
        SubText.text = "";

        TalkPanel.SetActive(false);
        LookAtScript.IKActive = false;

        PlayerVcam.enabled = true;
        TalkZoomVcam.enabled = false;
        RedFriendZoomVcam.enabled = false;

        FpsController.enabled = true;
        CanInteract = !endDay;

        yield return new WaitForSeconds(0.5f);
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
