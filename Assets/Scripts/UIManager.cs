using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
  [SerializeField] private GameObject ChooseLevelPage;
  [SerializeField] private GameObject WinPage;
  [SerializeField] private GameObject LosePage;

  [SerializeField] private TextMeshProUGUI moveCountText;
  [SerializeField] private TextMeshProUGUI gameLevelText;
  [SerializeField] private Button playButton;
  [SerializeField] private TMP_InputField levelIndexInputField;

  private void Awake() {
    playButton.onClick.AddListener(() => {
      PlayGame();
    });
  }

  private void PlayGame() {
    GameManager.Instance.PlayGame(levelIndexInputField.text);
    ChooseLevelPage.SetActive(false);
  }

  private void Start() {
    GameManager.Instance.OnGameOver += GameManager_OnGameOver;
    GameManager.Instance.OnMakeMove += GameManager_OnMakeMove;
    GameManager.Instance.GameManagerReady += GameManager_GameManagerReady;
  }

  private void GameManager_GameManagerReady(object sender, System.EventArgs e) {
    gameLevelText.text = $"Level {GameManager.Instance.GameLevel}";
    moveCountText.text = $"{GameManager.Instance.MoveCount} Moves";
  }

  private void OnDestroy() {
    GameManager.Instance.OnGameOver -= GameManager_OnGameOver;
    GameManager.Instance.OnMakeMove -= GameManager_OnMakeMove;
  }

  private void GameManager_OnMakeMove(object sender, System.EventArgs e) {
    moveCountText.text = $"{GameManager.Instance.MoveCount} Moves";
  }

  private void GameManager_OnGameOver(object sender, GameManager.OnGameOverEventArgs e) {
    if (e.win) {
      WinPage.SetActive(true);
    } else {
      LosePage.SetActive(true);
    }
    ChooseLevelPage.SetActive(true);
  }
}