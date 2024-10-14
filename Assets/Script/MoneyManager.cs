using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Include TextMeshPro namespace

public class MoneyManager : MonoBehaviour
{
    public int currentMoney = 0; 
    public TMP_Text moneyText; // Change to TMP_Text

    private void Start()
    {
        UpdateMoneyText();
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount; 
        UpdateMoneyText(); 
    }

    private void UpdateMoneyText()
    {
        moneyText.text = "" + currentMoney.ToString(); 
    }
}
