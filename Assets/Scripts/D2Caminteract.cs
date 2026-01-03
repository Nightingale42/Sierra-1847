using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class D2Caminteract : MonoBehaviour
{
    public bool TalkToActualFriend = false;
    public bool TalkToRedFriend = false;

    public LookAt3 LookAtScript;
    public Text InteractionText;
    public float InteractDistance = 2f;
    public bool CanInteract = true;

    public static int count;

    public CinemachineVirtualCamera PlayerVcam;
    public CinemachineVirtualCamera TalkZoomVcam;
    public CinemachineVirtualCamera RedFriendZoomVcam;
    public NewPlayerMovement FpsController;

    public GameObject TalkPanel;
    public GameObject ChoicePack;

    public GameObject ForagedGoodsBowl;
    public AudioClip BowlSpawnSound;

    private AudioSource audioSource;

    public Text SubText;

    string holder;
    float time = 0.05f;

    private D2SleepSystem sleepSystem;

    void Start()
    {
        sleepSystem = GetComponent<D2SleepSystem>();

        // Ensure we have an AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (!CanInteract)
            return;

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, InteractDistance))
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
            if (count > 5)
            {
                InteractionText.text = "Talk to Diana";

                if (Input.GetKeyDown(KeyCode.E))
                {
                    CanInteract = false;
                    StartCoroutine(TalkToRedFriendCO());
                }
            }
            else
            {
                InteractionText.text = "They don't seem interested.";
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
        TalkToRedFriend = true;
        InteractionText.text = "";
        FpsController.enabled = false;

        // Spawn bowl + play sound
        if (ForagedGoodsBowl != null)
            ForagedGoodsBowl.SetActive(true);

        if (audioSource != null && BowlSpawnSound != null)
            audioSource.PlayOneShot(BowlSpawnSound);

        RedFriendZoomVcam.enabled = true;
        PlayerVcam.enabled = false;
        TalkZoomVcam.enabled = false;

        LookAtScript.IKActive = true;

        yield return new WaitForSeconds(1f);

        TalkPanel.SetActive(true);

        SubText.text = "Me: ";
        holder = "Hey there. I just picked these out in the woods, do you know if they're safe to eat?";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();

        SubText.text = "Diana: ";
        holder = "That mushroom is an Amnita, it's pretty poisonous so I wouldn't recomend eating it. Honestly I wouldn't recommend eating anything in that basket thats touched it, but we both know out situation is too dire for percautions.";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();


        
        SubText.text = "Me: ";
        holder = "Okay, so basically everything but the mushroom is fine?";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();


         SubText.text = "Diana: ";
        holder = "Relatively.";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();


        SubText.text = "Me: ";
        holder = "Good enough for me.";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();

        StartCoroutine(FinalCO());
    }

    // ================= MAIN FRIEND =================
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

        if (D2EatingSystem.SliceEaten > 1)
        {
            if (sleepSystem != null)
                sleepSystem.enabled = true;

            SubText.text = "Friend: ";
            holder = "So how were they? Edible?";
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
            holder = "Oh nice! You got some stuff. I can't really tell what's safe or not... Oh, I know! You should go talk to Diana. She's able to recognize lots of forageables.";
            foreach (char c in holder)
            {
                SubText.text += c;
                yield return new WaitForSeconds(time);
            }

            yield return new WaitForSeconds(2f);
            StartCoroutine(FinalCO());
            yield break;
        }

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

    // ================= CHOICES =================
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

    IEnumerator FinalCO()
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

    IEnumerator MousePress()
    {
        while (!Input.GetMouseButtonDown(0))
            yield return null;
    }
}
