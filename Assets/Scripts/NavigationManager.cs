using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationManager : MonoBehaviour
{
    public void PlayGame() {
      SceneManager.LoadScene("GameScene");
    }
}