using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class MoneyUI : MonoBehaviour
{
    public Text moneyText;
    public Text energyText;

    void Update()
    {
        moneyText.text = PlayerStats.Money.ToString();
        energyText.text = PlayerStats.Energy.ToString();
    }
}
