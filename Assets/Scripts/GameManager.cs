using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  public GameObject endGameCanvas;
  private int zombiesRemaining;

  void Start()
  {
    // Make sure UI is hidden at start
    endGameCanvas.SetActive(false);

    // Register to listen for enemy death events
    Messenger.AddListener(GameEvent.ENEMY_HIT, OnEnemyKilled);

    // Count initial zombies (with a delay to ensure all are spawned)
    StartCoroutine(CountZombiesAfterDelay());
  }

  void OnDestroy()
  {
    // Clean up event listeners
    Messenger.RemoveListener(GameEvent.ENEMY_HIT, OnEnemyKilled);
  }

  IEnumerator CountZombiesAfterDelay()
  {
    // Wait for all zombies to spawn
    yield return new WaitForSeconds(2f);

    // Count zombies in scene
    zombiesRemaining = GameObject.FindGameObjectsWithTag("Enemy").Length;
    Debug.Log("Starting with " + zombiesRemaining + " zombies.");
  }

  void OnEnemyKilled()
  {
    zombiesRemaining--;
    Debug.Log("Zombie killed! Remaining: " + zombiesRemaining);

    // Check if all zombies are eliminated
    if (zombiesRemaining <= 0)
    {
      ShowVictoryScreen();
    }
  }

  void ShowVictoryScreen()
  {
    // Show end game UI
    endGameCanvas.SetActive(true);

    // Enable cursor for button clicking
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;

    // Disable player movement (optional)
    GameObject player = GameObject.FindWithTag("Player");
    if (player != null)
    {
      MonoBehaviour[] scripts = player.GetComponents<MonoBehaviour>();
      foreach (MonoBehaviour script in scripts)
      {
        if (script != this) // Don't disable this script
          script.enabled = false;
      }
    }
  }

  // Call this from Play Again button
  public void RestartGame()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }

  // Call this from Quit Game button
  public void QuitGame()
  {
    Debug.Log("Quitting game...");
    Application.Quit();

    // This line helps when testing in Unity Editor
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#endif
  }
}