using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance { get; private set; }

    private TextMeshProUGUI _survivedWaveText;

    private void Awake()
    {
        Instance = this;

        _survivedWaveText = transform.Find("wavesSurvivedText").GetComponent<TextMeshProUGUI>();
        transform.Find("retryBtn").GetComponent<Button>().onClick.AddListener(() => {
            GameSceneManager.Load(GameSceneManager.Scene.GameScene); });
        
        transform.Find("mainMenuBtn").GetComponent<Button>().onClick.AddListener(() => {
            GameSceneManager.Load(GameSceneManager.Scene.MainMenuScene);
        });

        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);

        _survivedWaveText.SetText($"You Survived {EnemyWaveManager.Instance.GetWaveNumber()} Waves!");

    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }


}
