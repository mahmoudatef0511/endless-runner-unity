using TMPro;
using UnityEngine;

public class UI_EndGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI coinsText;

    private void Start()
    {
        scoreText.text = "Score: ";
        coinsText.text = "Coins: ";

        int coins = GameManager.instance.coins;
        float score = GameManager.instance.distance;

        scoreText.text += score >= 1f ?
            score.ToString("#,#") + " m" : "0 m";
        coinsText.text += coins != 0 ?
            coins.ToString("#,#") : "0";
    }

}
