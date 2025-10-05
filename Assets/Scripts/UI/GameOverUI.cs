using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    
    [SerializeField] private Button retryButton;
    [SerializeField] private Button levelSelectButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        retryButton.onClick.AddListener((() =>
        {
            var currentScene = SceneManager.GetActiveScene().name;
            if (currentScene == "GameScene1")
            {
                Loader.Load(Loader.Scene.GameScene1);
            }else if (currentScene == "GameScene2")
            {
                Loader.Load(Loader.Scene.GameScene2);
            }else if (currentScene == "GameScene3")
            {
                Loader.Load(Loader.Scene.GameScene3);
            }
        }));
        levelSelectButton.onClick.AddListener((() =>
            {
                Loader.Load(Loader.Scene.LevelSelectScene);
            }));
        mainMenuButton.onClick.AddListener((() =>
            {
                Loader.Load(Loader.Scene.MainMenuScene);
            }));
    }
    
    private void Start()
    {
        GameHandler.Instance.OnStateChanged += GameHandler_OnStateChanged;

        Hide();
    }

    private void GameHandler_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameHandler.Instance.IsGameOver())
        {
            Show();

            int successfulRecipes = DeliveryManager.Instance.GetSuccessfulRecipesAmount();
            recipesDeliveredText.text = successfulRecipes.ToString();

            if (successfulRecipes > PlayerPrefs.GetInt(SceneManager.GetActiveScene().name+"MaxScore"))
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name+"MaxScore", successfulRecipes);
            }
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }

}
