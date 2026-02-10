using UnityEngine;

public class LookAtFunction : MonoBehaviour
{
    [Header("Animators")]
    public Animator animator;
    public Animator redFriendAnimator;

    [Header("Look Settings")]
    public bool IKActive = false;
    public Transform LookAtObj;
    [Range(0f, 1f)]
    public float LookWeight = 0f;
    public float LookSpeed = 2f;

    private void OnAnimatorIK(int layerIndex)
    {
        if (!IKActive || LookAtObj == null)
        {
            LookWeight = Mathf.Lerp(LookWeight, 0f, Time.deltaTime * LookSpeed);
        }
        else
        {
            LookWeight = Mathf.Lerp(LookWeight, 1f, Time.deltaTime * LookSpeed);
        }

        if (animator != null)
        {
            animator.SetLookAtWeight(LookWeight);
            animator.SetLookAtPosition(LookAtObj.position);
        }

        if (redFriendAnimator != null)
        {
            redFriendAnimator.SetLookAtWeight(LookWeight);
            redFriendAnimator.SetLookAtPosition(LookAtObj.position);
        }
    }
}