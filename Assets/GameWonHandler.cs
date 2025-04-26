using UnityEngine;

public class GameWonHandler : MonoBehaviour
{
    [SerializeField] private GameObject heavenBoss;
    [SerializeField] private GameObject gameWonUI;
    [SerializeField] private string gameWonMusicName = "GameWon";

    private bool hasWon = false;
    public static bool IsGameWon { get; private set; } = false;  // Static flag to track game win

    private void Start()
    {
        if (gameWonUI != null)
        {
            gameWonUI.SetActive(false); // Hide GameWon UI initially
        }
    }

    private void Update()
    {
        if (!hasWon && heavenBoss != null && !heavenBoss.activeInHierarchy)
        {
            HandleGameWon();
        }
    }

    private void HandleGameWon()
    {
        hasWon = true;
        IsGameWon = true;  // Mark the game as won

        // Show Game Won UI
        if (gameWonUI != null)
        {
            gameWonUI.SetActive(true);
        }

        // Play Game Won music (only if MusicManager is safely accessible)
        if (MusicManager.Instance != null && !string.IsNullOrEmpty(gameWonMusicName))
        {
            MusicManager.Instance.PlayMusic(gameWonMusicName);
        }
        else
        {
            Debug.LogWarning("GameWonHandler: MusicManager or music name not available.");
        }
    }
}
