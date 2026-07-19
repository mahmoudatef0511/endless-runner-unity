using System.Collections;
using TMPro;
using UnityEngine;

public class InfoTextController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI infoText;

    public IEnumerator UpdateInfoText(string newText)
    {
        infoText.text = newText;
        yield return new WaitForSeconds(1.5f);
        infoText.text = "CLICK TO BUY";
    }

}
