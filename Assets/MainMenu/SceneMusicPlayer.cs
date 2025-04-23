using UnityEngine;

public class SceneMusicPlayer : MonoBehaviour
{
    [Header("Name of the music track to play")]
    [SerializeField] private string musicName;

    private void Start()
    {
        if (!string.IsNullOrEmpty(musicName))
        {
            MusicManager.Instance.PlayMusic(musicName);
        }
        else
        {
            Debug.LogWarning("SceneMusicPlayer: Music name not set!");
        }
    }
}
