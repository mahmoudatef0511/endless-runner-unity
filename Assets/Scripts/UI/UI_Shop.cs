using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalCoinsText;
    [SerializeField] private Image playerDisplay;
    [SerializeField] private Image platformDisplay;

    private void Start()
    {
        UpdateCoins();
        UpdatePlayerDisplay();
        UpdatePlatformDisplay();
    }
    private void UpdateCoins()
    {
        int totalCoins = PlayerPrefs.GetInt("Coins");
        totalCoinsText.text = totalCoins > 0 ?
            totalCoins.ToString("#,#") : "0";
    }

    public void UpdatePlayerDisplay()
    {
        UpdateCoins();
        float colorR = PlayerPrefs.GetFloat("PlayerColorR", 1);
        float colorG = PlayerPrefs.GetFloat("PlayerColorG", 1);
        float colorB = PlayerPrefs.GetFloat("PlayerColorB", 1);
        float colorA = PlayerPrefs.GetFloat("PlayerColorA", 1);
        playerDisplay.color = new Color(colorR, colorG, colorB, colorA);
    }

    public void UpdatePlatformDisplay()
    {
        UpdateCoins();
        float colorR = PlayerPrefs.GetFloat("PlatformColorR", 96f / 255f);
        float colorG = PlayerPrefs.GetFloat("PlatformColorG", 61f / 255f);
        float colorB = PlayerPrefs.GetFloat("PlatformColorB", 38f / 255f);
        float colorA = PlayerPrefs.GetFloat("PlatformColorA", 1f);
        platformDisplay.color = new Color(colorR, colorG, colorB, colorA);
    }
}
