using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    Player,
    Platform
}
public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject itemBoughtFlag;
    [SerializeField] private ItemType itemType;

    private Color itemColor;
    private string slotName;
    private bool isBought;

    private void Start()
    {
        LoadItemSlot(itemType);
    }

    private void LoadItemSlot(ItemType itemType)
    {
        slotName = gameObject.name;
        if (itemType == ItemType.Platform)
        {
            itemColor = GameManager.instance.platformColor;
            if (slotName == "PlatformColorSlot1") isBought = true;
            else isBought = PlayerPrefs.GetInt(slotName, 0) == 1;
        }
        else if (itemType == ItemType.Player)
        {
            itemColor = GameManager.instance.playerColor;
            if (slotName == "PlayerColorSlot1") isBought = true;
            else isBought = PlayerPrefs.GetInt(slotName, 0) == 1;
        }
        itemBoughtFlag.SetActive(isBought);
    }

    public void Buy()
    {
        AudioManager.instance.PlaySFX(4);
        Color itemSlotColor = itemIcon.color;
        int price = int.Parse(priceText.text);
        int coins = PlayerPrefs.GetInt("Coins");
        InfoTextController infoText = transform.parent.GetComponent<InfoTextController>();

        if (isBought)
        {
            itemColor = itemSlotColor;
            if (itemType == ItemType.Platform)
            {
                GameManager.instance.SavePlatformColor(
                itemColor.r, itemColor.g, itemColor.b);
                GameManager.instance.LoadPlatformColor();
            }
            else if (itemType == ItemType.Player)
            {
                GameManager.instance.SavePlayerColor(
                itemColor.r, itemColor.g, itemColor.b);
                GameManager.instance.LoadPlayerColor();
            }
            return;
        }

        if (coins >= price)
        {
            isBought = true;
            itemBoughtFlag.SetActive(isBought);
            itemColor = itemSlotColor;
            PlayerPrefs.SetInt(slotName, 1);
            PlayerPrefs.SetInt("Coins", coins - price);
            if (itemType == ItemType.Platform)
            {
                GameManager.instance.SavePlatformColor(
                itemColor.r, itemColor.g, itemColor.b);
                GameManager.instance.LoadPlatformColor();
            }
            else if (itemType == ItemType.Player)
            {
                GameManager.instance.SavePlayerColor(
                itemColor.r, itemColor.g, itemColor.b);
                GameManager.instance.LoadPlayerColor();
            }
            StartCoroutine(infoText.UpdateInfoText("PURCHASED SUCCESSFULLY!"));
        }
        else
            StartCoroutine(infoText.UpdateInfoText("NO ENOUGH COINS!"));

    }
}
