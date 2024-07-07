using UnityEngine;

public class MusicManager : MonoBehaviour {
  public static MusicManager Instance { get; private set; }

  [SerializeField] private float volume = 0.05f;

  AudioSource audioSource;
  bool isMuted = false;

  void Awake() {
    if (Instance == null) {
      Instance = this;
      DontDestroyOnLoad(gameObject);
      audioSource = GetComponent<AudioSource>();
      audioSource.volume = volume;
    } else if (Instance != this) {
      Destroy(gameObject);
    }
  }

  public float GetVolume() {
    return volume;
  }

  public void ToggleMute() {
    if (isMuted) {
      audioSource.volume = 0.05f;
      isMuted = false;
    } else {
      audioSource.volume = 0f;
      isMuted = true;
    }
  }
}