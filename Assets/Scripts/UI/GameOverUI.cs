using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;

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
