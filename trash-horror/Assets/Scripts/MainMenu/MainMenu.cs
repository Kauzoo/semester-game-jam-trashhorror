using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button startButton, quitButton;

    public String defaultScene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startButton.onClick.AddListener(LoadDefaultScene);
        quitButton.onClick.AddListener(QuitGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadDefaultScene()
    {
        SceneManager.LoadScene(defaultScene);
    }
    
    private void QuitGame()
    {
        Application.Quit();
    }
}
