using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    private TextMeshProUGUI _soundText;
    private TextMeshProUGUI _musicText;

    private void Awake()
    {
        _soundText = transform.Find("soundVolumeText").GetComponent<TextMeshProUGUI>();
        _musicText = transform.Find("musicVolumeText").GetComponent<TextMeshProUGUI>();

        transform.Find("soundIncreaseBtn").GetComponent<Button>().onClick.AddListener(() => 
        { 
            SoundManager.Instance.IncreaseVolume();
            UpdateText();
        });
        
        transform.Find("soundDecreaseBtn").GetComponent<Button>().onClick.AddListener(() => 
        { 
            SoundManager.Instance.DecreaseVolume();
            UpdateText();
        });
        
        transform.Find("musicIncreaseBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            MusicManager.Instance.IncreaseVolume();
            UpdateText();
        });

        transform.Find("musicDecreaseBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            MusicManager.Instance.DecreaseVolume();
            UpdateText();
        });

        transform.Find("mainMenuBtn").GetComponent<Button>().onClick.AddListener(() => { });
    }

    private void Start()
    {
        UpdateText();  
        gameObject.SetActive(false);
    }

    private void UpdateText ()
    {
        _soundText.SetText(Mathf.RoundToInt(SoundManager.Instance.GetVolume() * 10).ToString());
        _musicText.SetText(Mathf.RoundToInt(MusicManager.Instance.GetVolume() * 10).ToString());
    }

    public void ToggleVisible()
    {
        gameObject.SetActive( !gameObject.activeSelf );  

        //Pause the game when Options Menu is active, othwerwise resume game
        if (gameObject.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }


}
