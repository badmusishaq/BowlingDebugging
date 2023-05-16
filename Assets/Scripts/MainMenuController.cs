using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button quitButton;

    public void OnStartGameClicked()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void OnQuitGameClicked()
    {
        Application.Quit(); // No effect in Editor
    }

    private void Start()
    {
        quitButton.onClick.AddListener(OnQuitGameClicked);
    }
}
