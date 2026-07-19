using UnityEngine;

public class CoinGenerator : MonoBehaviour
{

    private int coinsAmount;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int minCoins;
    [SerializeField] private int maxCoins;
    [SerializeField] private SpriteRenderer[] coinImgs;

    void Start()
    {

        for (int i = 0; i < coinImgs.Length; i++)
        {
            coinImgs[i].sprite = null;
        }

        coinsAmount = Random.Range(minCoins, maxCoins + 1);
        int additionalOffset = coinsAmount / 2;

        for (int i = 0; i < coinsAmount; i++)
        {
            Vector3 offset = new Vector3(i - additionalOffset, 0, transform.position.z);
            Instantiate(coinPrefab, offset + transform.position, Quaternion.identity, transform);
        }
        
    }
}
