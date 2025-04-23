using UnityEngine;

public class PlayOnceAndDestroy : MonoBehaviour
{
    private Animator animator;
    private bool hasPlayed = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogWarning("Animator not found on this GameObject.");
            Destroy(gameObject); // Optionally destroy if no animation can play
            return;
        }

        // Play the first animation (or specify one with .Play("AnimationName"))
        animator.Play(0);
        hasPlayed = true;

        // Get the animation clip duration and start coroutine to destroy after it finishes
        float clipLength = animator.GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(DestroyAfterAnimation(clipLength));
    }

    private System.Collections.IEnumerator DestroyAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
