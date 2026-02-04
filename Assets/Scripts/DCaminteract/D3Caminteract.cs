using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class D3Caminteract : MonoBehaviour
{
    public bool TalkToActualFriend = false;
    public bool TalkToRedFriend = false;

    public LookAt4 LookAtScript;
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

    private D3SleepSystem sleepSystem;

    void Start()
    {
        sleepSystem = GetComponent<D3SleepSystem>();

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

        if (D3EatingSystem.SliceEaten > 1)
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
            holder = "Thanks for getting those logs. We need to get these cabins up now.";

              foreach (char c in holder)
            {
                SubText.text += c;
                yield return new WaitForSeconds(time);
            }

            yield return MousePress();

             if (sleepSystem != null)
                sleepSystem.enabled = true;

            yield return new WaitForSeconds(2f);
            StartCoroutine(FinalCO());
            yield break;
        }

        SubText.text = "Friend: ";
        holder = "Well.";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();

        SubText.text = "Me: ";
        holder = "I think it's dead?";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();

        SubText.text = "Friend: ";
        holder = "Oh you think so?";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

    yield return MousePress();

     SubText.text = "Me: ";
        holder = "Shut up.";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();



         SubText.text = "Friend: ";
        holder = "At least we'll eat tonight I guess. On the downside...";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return MousePress();

         SubText.text = "Friend: ";
        holder = "We're stranded.";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        } 

        yield return MousePress();


         SubText.text = "Me: ";
        holder = "With no food.";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        } 

        yield return MousePress();


        
         SubText.text = "Friend: ";
        holder = "Well I mean... We've got the horse.";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        } 

        yield return MousePress();

        
         SubText.text = "Me: ";
        holder = "Yeah man, I guess...";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        } 

        yield return MousePress();

        
         SubText.text = "Friend: ";
        holder = "We need a game plan. I need you to go collect logs- we're going to build cabins.";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        } 

        yield return MousePress();

        
         SubText.text = "Me: ";
        holder = "We're not even going to try to get out?";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        } 

        yield return MousePress();

        
         SubText.text = "Friend: ";
        holder = "You and what army? We have no food! no horse!";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        } 

        yield return MousePress();

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
        holder = "Fine, fine. I'll go get the logs.";
        foreach (char c in holder)
        {
            SubText.text += c;
            yield return new WaitForSeconds(time);
        }

        yield return new WaitForSeconds(1f);

        SubText.text = "Friend: ";
        holder = "Come back to me when you're done.";
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
        holder = "I need a minute.";
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
