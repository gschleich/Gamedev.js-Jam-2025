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
            playerHealth.OnDeathWithReference.AddListener(OnPlayerDeath);
        }

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false); // Hide GameOver UI initially
        }
    }

    private void OnPlayerDeath(GameObject sender)
    {
        // Activate Game Over UI
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        // Play Game Over music
        if (!string.IsNullOrEmpty(gameOverMusicName))
        {
            MusicManager.Instance.PlayMusic(gameOverMusicName);
        }
        else
        {
            Debug.LogWarning("GameOverHandler: Game Over music name not set!");
        }
    }
}
