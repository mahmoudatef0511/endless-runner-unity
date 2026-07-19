using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Main : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI lastScoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject endGameMenu;

    [Header("Volume Info:")]
    [SerializeField] private VolumeSliderController[] volumeSliders;
    [SerializeField] private Image muteIcon;
    [SerializeField] private Image inGameMuteIcon;

    private bool gamePaused;
    private bool gameMuted;

    private void Start()
    {
        for (int i = 0; i < volumeSliders.Length; i++)
        {
            volumeSliders[i].SetupSlider();
        }

        lastScoreText.text = "Last Score: ";
        bestScoreText.text = "Best Score: ";

        int coins = PlayerPrefs.GetInt("Coins");
        float lastScore = PlayerPrefs.GetFloat("LastScore"),
              bestScore = PlayerPrefs.GetFloat("BestScore");

        coinsText.text = coins != 0 ? 
            coins.ToString("#,#") : "0";
        lastScoreText.text += lastScore >= 1f ?
            lastScore.ToString("#,#") + " m" : "0 m";
        bestScoreText.text += bestScore >= 1f ?
            bestScore.ToString("#,#") + " m" : "0 m";

        EnableMenu(mainMenu);
        GameManager.instance.LoadPlayerColor();
        GameManager.instance.LoadPlatformColor();
    }

    public void SwitchDayNight(int index)
    {
        GameManager.instance.SetupSkyBox(index);
    }

    public void EnableMenu(GameObject menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        AudioManager.instance.PlaySFX(4);
        menu.SetActive(true);
    }
    public void StartGame()
    {
        inGameMuteIcon.color = muteIcon.color;
        muteIcon = inGameMuteIcon;
        GameManager.instance.UnlockPlayer();
    }
    public void PauseGame()
    {
        if (gamePaused)
        {
            Time.timeScale = 1;
            gamePaused = false;
        }
        else
        {
            Time.timeScale = 0;
            gamePaused = true;
        }
    }
    public void MuteGame()
    {
        Color muteColor = new Color(1, 1, 1, 0.3f);
        gameMuted = !gameMuted;
        if (gameMuted)
        {
            muteIcon.color = muteColor;
            AudioListener.volume = 0;
        }
        else
        {
            muteIcon.color = Color.white;
            AudioListener.volume = 1;
        }
    }
    public void RestartGame() => GameManager.instance.RestartLevel();
    public void EndGame()
    {
        AudioManager.instance.StopBGM();
        AudioManager.instance.PlaySFX(3);
        EnableMenu(endGameMenu);
    }
}
