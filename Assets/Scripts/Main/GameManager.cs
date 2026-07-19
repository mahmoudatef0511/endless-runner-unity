using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player player;

    public UI_Main uiMain;

    [Header("SkyBox Materials Info:")]
    [SerializeField] private Material[] skyBoxMaterials;

    [Header("Color Info:")]
    public Color playerColor;
    public Color platformColor; 
    public Color platformHeaderColor;

    [Header("Score Info:")]
    public float distance;
    public int coins;

    private void Awake()
    {
        instance = this;
        Time.timeScale = 1;
        SetupSkyBox(PlayerPrefs.GetInt("SkyBoxSetting"));
    }

    private void Start()
    {
        LoadPlayerColor();
        LoadPlatformColor();
    }

    public void SetupSkyBox(int index)
    {
        if(index <= 1)
            RenderSettings.skybox = skyBoxMaterials[index];
        else
            RenderSettings.skybox = 
                skyBoxMaterials[Random.Range(0, skyBoxMaterials.Length)];
        PlayerPrefs.SetInt("SkyBoxSetting", index);
    }

    public void SavePlayerColor(float r, float g, float b)
    {
        PlayerPrefs.SetFloat("PlayerColorR", r);
        PlayerPrefs.SetFloat("PlayerColorG", g);
        PlayerPrefs.SetFloat("PlayerColorB", b);
    }
    public void LoadPlayerColor()
    {
        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        float colorR = PlayerPrefs.GetFloat("PlayerColorR", 1);
        float colorG = PlayerPrefs.GetFloat("PlayerColorG", 1);
        float colorB = PlayerPrefs.GetFloat("PlayerColorB", 1);
        float colorA = PlayerPrefs.GetFloat("PlayerColorA", 1);
        sr.color = new Color(colorR, colorG, colorB, colorA);
        playerColor = sr.color;
    }
    public void SavePlatformColor(float r, float g, float b)
    {
        PlayerPrefs.SetFloat("PlatformColorR", r);
        PlayerPrefs.SetFloat("PlatformColorG", g);
        PlayerPrefs.SetFloat("PlatformColorB", b);
    }
    public void LoadPlatformColor()
    {
        float colorR = PlayerPrefs.GetFloat("PlatformColorR", 96f / 255f);
        float colorG = PlayerPrefs.GetFloat("PlatformColorG", 61f / 255f);
        float colorB = PlayerPrefs.GetFloat("PlatformColorB", 38f / 255f);
        float colorA = PlayerPrefs.GetFloat("PlatformColorA", 1f);
        platformColor = new Color(colorR, colorG, colorB, colorA);
    }

    public void UnlockPlayer() => player.isMoving = true;

    private void Update()
    {
        if(player.transform.position.x > distance)
            distance = player.transform.position.x;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }
    public void Save()
    {
        int savedCoins = PlayerPrefs.GetInt("Coins", 0);
        float savedLastScore = PlayerPrefs.GetFloat("LastScore", 0),
              savedBestScore = PlayerPrefs.GetFloat("BestScore", 0);

        if (distance > savedBestScore) savedBestScore = distance;

        PlayerPrefs.SetFloat("LastScore", 
            distance == 0 ? savedLastScore : distance);
        PlayerPrefs.SetFloat("BestScore", savedBestScore);
        PlayerPrefs.SetInt("Coins", savedCoins + coins);
    }

    public void GameEnded()
    {
        Save();
        uiMain.EndGame();
    }
}
