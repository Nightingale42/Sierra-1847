using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
using UnityEngine.SceneManagement;

public class WagonCamInteract : MonoBehaviour
{
    // ================= INTERACTION =================
    [Header("Interaction")]
    public Transform RayOrigin;

    public bool choiceMade = false;
    public int chosenOption = -1;

    public Text InteractionText;
    public float InteractDistance = 1f;
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
    public float WriteSpeed = .004f;
    public int NextSceneIndex = 1;

    // ================= STATE =================
    private float typeSpeed = 0.04f;
    private bool endingDay;

    // ================= WOOD =================
    public static int count;
public void MakeChoice(int option)
{
    chosenOption = option;
    choiceMade = true;
}
    // --------------------------------------------------
    void Update()
    {
        // If interaction is disabled, force-clear text
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
            // -------- DEFAULT CONVERSATION --------
            yield return TypeLine("You: ",
                "I'm so ready to get out of here. I know the trip is short but I could not wait to leave.");
            yield return WaitForClick();

            yield return TypeLine("Jesse: ",
                "No kidding- I swear my family was fixin to run me plum up the wall. Speaking of, why’re you heading west?");
            yield return WaitForClick();

           // -------- CHOICE PAUSE --------



    ChoicePack.SetActive(true);

    // Wait until player selects anything
    yield return new WaitUntil(() => choiceMade == true);

    ChoicePack.SetActive(false);

    // reset (important if reused later)
    choiceMade = false;
    chosenOption = -1;



    // -------- SAME RESPONSE REGARDLESS OF CHOICE --------

            yield return TypeLine("Jesse: ",
                "That's as good a reason as any I suppose.");
            yield return WaitForClick();

              yield return TypeLine("Jesse: ",
                "...");
            yield return WaitForClick();

              yield return TypeLine("Jesse: ",
                "Sure hope Virgil knows what he’s doing. What with this being the first time anyone has ever traveled this route and all.");
            yield return WaitForClick();

             yield return TypeLine("You: ",
                "He’s got that fancy book of his, right? Thing’s got the path all mapped out.");
            yield return WaitForClick();

             yield return TypeLine("Jesse: ",
                "Well... we sure didn’t leave ourselves much room for error. Reckon it’ll turn out alright though—he keeps a tight hold on the reins.");
            yield return WaitForClick();

           // ChoicePack.SetActive(true);
            yield return ExitDialogue(true);
            yield break;
            
        }

        // -------- END DAY --------
        yield return TypeLine("Friend: ",
            "Probably best to head to bed. We've got a big day ahead.");

        yield return new WaitForSeconds(2f);
        yield return ExitDialogue(true);
    }

    // ================= CHOICES =================


   

    // ================= EXIT =================
    IEnumerator ExitDialogue(bool endDay)
    {
        if (endingDay) yield break;
        endingDay = endDay;

        // HARD CLEAR INTERACTION TEXT
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

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(NextSceneIndex);
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
