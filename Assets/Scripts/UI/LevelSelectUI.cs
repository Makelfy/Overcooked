using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] private Button level1SelectButton;
    [SerializeField] private Button level2SelectButton;
    [SerializeField] private Button level3SelectButton;

    [SerializeField] private TextMeshProUGUI level1MaxScoreText;
    [SerializeField] private TextMeshProUGUI level2MaxScoreText;
    [SerializeField] private TextMeshProUGUI level3MaxScoreText;
    
    private void Awake()
    {
        level1SelectButton.onClick.AddListener((() =>
        {
            Loader.Load(Loader.Scene.GameScene1);
        }));
        level2SelectButton.onClick.AddListener((() =>
        {
            Loader.Load(Loader.Scene.GameScene2);
        }));        
        level3SelectButton.onClick.AddListener((() =>
        {
            Loader.Load(Loader.Scene.GameScene3);
        }));
    }

    private void Start()
    {
        level1MaxScoreText.text = "Max Food\nDelivered: " + PlayerPrefs.GetInt("GameScene1MaxScore").ToString();
        level2MaxScoreText.text = "Max Food\nDelivered: " + PlayerPrefs.GetInt("GameScene2MaxScore").ToString();
        level3MaxScoreText.text = "Max Food\nDelivered: " + PlayerPrefs.GetInt("GameScene3MaxScore").ToString();
    }
}
