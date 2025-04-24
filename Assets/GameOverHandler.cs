using UnityEngine;

public class GameOverHandler : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private string gameOverMusicName = "GameOver";

    private void Start()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDeathWithReference.AddListener(HandleGameOver);
        }

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false); // Hide GameOver UI initially
        }
    }

    private void HandleGameOver(GameObject sender)
    {
        // Show Game Over UI
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        // Play Game Over music (only if MusicManager is safely accessible)
        if (MusicManager.Instance != null && !string.IsNullOrEmpty(gameOverMusicName))
        {
            MusicManager.Instance.PlayMusic(gameOverMusicName);
        }
        else
        {
            Debug.LogWarning("GameOverHandler: MusicManager or music name not available.");
        }
    }
}
