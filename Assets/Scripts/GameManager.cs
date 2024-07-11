using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
  public static GameManager Instance { get; private set; }

  public event EventHandler OnMakeMove;

  public event EventHandler<OnGameOverEventArgs> OnGameOver;

  public class OnGameOverEventArgs : EventArgs {
    public bool win;
  }

  [SerializeField] private List<LevelSO> levelList;

  [SerializeField] private LayerMask mouseColliderLayerMask = new LayerMask();
  [SerializeField] private float duration = 1f;

  private int currentLevelIndex = 0;
  public int MoveCount { get; private set; }
  public int GameLevel { get; private set; }

  private void Awake() {
    if (Instance == null) {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    } else if (Instance != this) {
      Destroy(gameObject);
    }
  }

  void Start() {
    MoveCount = levelList[currentLevelIndex].levelMoveCount;
    GameLevel = levelList[currentLevelIndex].gameLevel;
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

  public void PlayGame(string inputField) {
    if (int.TryParse(inputField, out int result)) {
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

  public void GetLevelWithIndex(int gameLevel) {
    currentLevelIndex = gameLevel;
    MoveCount = levelList[currentLevelIndex].levelMoveCount;
    GameLevel = levelList[currentLevelIndex].gameLevel;
    // Get the active scene
    Scene currentScene = SceneManager.GetActiveScene();
    // Reload the current scene
    SceneManager.LoadScene(currentScene.name);
  }

  public void DecreaseMoveCount() {
    MoveCount--;
    if (MoveCount <= 0) {
      // game is over
      GameOver(win: false);
    }
    OnMakeMove.Invoke(this, EventArgs.Empty);
  }

  public void GameOver(bool win) {
    OnGameOver.Invoke(this, new() {
      win = win
    });
  }
}