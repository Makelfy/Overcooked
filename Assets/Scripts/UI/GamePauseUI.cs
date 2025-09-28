using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionsButton;


    private void Awake()
    {
        resumeButton.onClick.AddListener((() =>
        {
            GameHandler.Instance.TogglePauseGame();
        }));
        mainMenuButton.onClick.AddListener((() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        }));
        optionsButton.onClick.AddListener((() =>
        {
            Hide();
            OptionsUI.Instance.Show(Show);
        }));
    }
    private void Start()
    {
        GameHandler.Instance.OnGamePaused += GameHandler_OnGamePaused;
        GameHandler.Instance.OnGameUnpaused += GameHandler_OnGameUnpaused;
        
        Hide();
    }

    private void GameHandler_OnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void GameHandler_OnGamePaused(object sender, EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        
        resumeButton.Select();
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
