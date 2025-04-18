using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnTrigger : MonoBehaviour
{
    public Animator transition;

    [Tooltip("Name of the scene to load on trigger with the Player.")]
    public string sceneToLoad;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(LoadSceneAfterDelay(1f));
        }
    }

    private System.Collections.IEnumerator LoadSceneAfterDelay(float delay)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(delay);

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Scene to load is not set.");
        }
    }
}
