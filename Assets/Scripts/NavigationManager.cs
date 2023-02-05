using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NavigationManager : MonoBehaviour
{
    public static bool GameOver = false;
    public static bool newRecord = false;
    public static int score = 0;

    public TMP_Text headerText;
    public TMP_Text subHeaderText;
    public TMP_Text buttonText;
    public Transform tree;

    private float treeMinSize = 2.0f;
    private float treeMaxSize = 10.0f;

    private float scoreForMaxTreeSize = 500.0f;

    float GetTreeSize (float _score) {
      return treeMinSize + treeMaxSize * Mathf.Min((float) _score / scoreForMaxTreeSize, 1);
    }

    void Start() {
      if (GameOver) {
        headerText.text = "Game Over";

        if (newRecord == true) {
          subHeaderText.text = "New record! Score: " + score;
        } else {
          subHeaderText.text = "Score: " + score;
        }

        buttonText.text = "Play Again";

        float treeSize = GetTreeSize(score);

        tree.transform.localScale = new Vector3(treeSize, treeSize, 1.0f);
      } else {
        headerText.text = "Root runner";

        if (PlayerPrefs.HasKey("highscore") == true) {
          float treeSize = GetTreeSize(score);
          tree.transform.localScale = new Vector3(treeSize, treeSize, 1.0f);

          subHeaderText.text = "Highscore: " + PlayerPrefs.GetInt("highscore");
        }

        buttonText.text = "Play";
      }
    }

    void Update() {
      if (GameOver) {
        if (Input.GetKeyDown(KeyCode.Space)) {
          SceneManager.LoadScene("GameScene");
        }
      }
    }
    
    public void PlayGame() {
      SceneManager.LoadScene("GameScene");
    }
}
