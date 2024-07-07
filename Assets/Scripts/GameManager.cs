using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
  public static GameManager Instance { get; private set; }

  [SerializeField] private List<LevelSO> levelList;

  [SerializeField] private GameObject ChooseLevelPage;
  [SerializeField] private GameObject WinPage;
  [SerializeField] private GameObject LosePage;

  [SerializeField] private TextMeshProUGUI moveCountText;
  [SerializeField] private TextMeshProUGUI gameLevelText;
  [SerializeField] private Button playButton;
  [SerializeField] private TMP_InputField levelIndexInputField;

  [SerializeField] private LayerMask mouseColliderLayerMask = new LayerMask();
  [SerializeField] private float duration = 1f;

  private int currentLevelIndex = 0;
  private int moveCount;
  private int gameLevel;

  private void Awake() {
    Instance = this;
    DontDestroyOnLoad(gameObject);

    playButton.onClick.AddListener(() => {
      PlayGame();
    });
  }

  private void PlayGame() {
    if (int.TryParse(levelIndexInputField.text, out int result)) {
      if (result > levelList.Count || result < 1) {
        return;
      }

      currentLevelIndex = result - 1;

      GetLevelWithIndex(currentLevelIndex);
    }
  }

  public float GetDuration() {
    return duration;
  }

  public LevelSO GetLevelSO() {
    return levelList[currentLevelIndex];
  }

  void Start() {
    moveCount = levelList[currentLevelIndex].levelMoveCount;
    gameLevel = levelList[currentLevelIndex].gameLevel;
    RefreshTexts();
  }

  private void RefreshTexts() {
    gameLevelText.text = $"Level {gameLevel}";
    moveCountText.text = $"{moveCount} Moves";
  }

  void Update() {
    if (Input.GetKeyDown(KeyCode.Mouse0)) {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderLayerMask)) {
        var cell = raycastHit.transform.GetComponent<Cell>();
        cell.StartCollecting();
      }
    }
  }

  public void GetLevelWithIndex(int gameLevel) {
    currentLevelIndex = gameLevel;
    // Get the active scene
    Scene currentScene = SceneManager.GetActiveScene();
    // Reload the current scene
    SceneManager.LoadScene(currentScene.name);
  }

  public void DecreaseMove() {
    moveCount--;
    if (moveCount <= 0) {
      // game is over
      GameOver(win: false);
    }

    RefreshTexts();
  }

  public void GameOver(bool win) {
    if (win) {
      // pop up win
      Debug.Log("you win");
      WinPage.SetActive(true);
    } else {
      // pop up lose
      Debug.Log("you lose");
      LosePage.SetActive(true);
    }

    ChooseLevelPage.SetActive(true);
  }
}