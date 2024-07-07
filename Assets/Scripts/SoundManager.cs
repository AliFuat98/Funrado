using UnityEngine;

public class SoundManager : MonoBehaviour {
  public static SoundManager Instance { get; private set; }

  [SerializeField] private AudioClipSO audioClipSO;

  private void Awake() {
    Instance = this;
  }

  public void Reward() {
    PlaySound(audioClipSO.reward, Vector3.zero);
  }

  public void CollectGrape(int index) {
    PlaySound(audioClipSO.collectGrape, Vector3.zero, index: index);
  }

  public void WrongMove() {
    PlaySound(audioClipSO.wrongMove, Vector3.zero);
  }

  public void GetBackTongue() {
    PlaySound(audioClipSO.getBackTongue, Vector3.zero);
  }

  private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f, int index = -1) {
    if (index == -1) {
      // choose random
      index = Random.Range(0, audioClipArray.Length);
    }

    PlaySound(audioClipArray[index], position, volume);
  }

  private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f) {
    AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier);
  }
}