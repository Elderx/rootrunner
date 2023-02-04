using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NavigationManager : MonoBehaviour
{
    public static bool GameOver = false;
    public TMP_Text headerText;
    public TMP_Text buttonText;

    void Start() {
      if (GameOver) {
        headerText.text = "Game Over";
        buttonText.text = "Play Again";
      } else {
        headerText.text = "Root runner";
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
