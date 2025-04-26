using UnityEngine;

public class MeterTracker : MonoBehaviour
{
    public static MeterTracker Instance;

    public RectTransform meterMarker;
    public GameObject gameOverUI;

    private float currentX = 0f;
    private const float limit = 450f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Avoid duplicates
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
    }

    public void AdjustMeter(int direction)
    {
        currentX += direction;

        if (meterMarker != null)
        {
            meterMarker.anchoredPosition = new Vector2(currentX, meterMarker.anchoredPosition.y);
        }

        if (Mathf.Abs(currentX) >= limit && gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
    }

    // Optional: call this if you need to sync marker after scene loads
    public void SyncMeter()
    {
        if (meterMarker != null)
        {
            meterMarker.anchoredPosition = new Vector2(currentX, meterMarker.anchoredPosition.y);
        }
    }
}
