using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI coinsText;

    [SerializeField] private Button jumpBtn;
    [SerializeField] private Button slideBtn;

    [SerializeField] private Image extraLife;
    [SerializeField] private Image noExtraLife;
    [SerializeField] private Image slide;

    private float distance;
    private int coins;
    private bool hasExtraLife;
    private bool canSlide;

    private void Start()
    {
        jumpBtn.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        slideBtn.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        extraLife.gameObject.SetActive(true);
        noExtraLife.gameObject.SetActive(false);
        InvokeRepeating("UpdateInfo", 0, 0.2f);

    }

    private void UpdateInfo()
    {
        distance = GameManager.instance.distance;
        coins = GameManager.instance.coins;
        hasExtraLife = GameManager.instance.player.hasExtraLife;
        canSlide = GameManager.instance.player.canSlide;

        if(distance > 0)
            distanceText.text = distance.ToString("#,#") + " m";

        if(coins > 0)
            coinsText.text = coins.ToString("#,#");

        extraLife.gameObject.SetActive(hasExtraLife);
        noExtraLife.gameObject.SetActive(!hasExtraLife);
        slide.gameObject.SetActive(canSlide);
    }

}
