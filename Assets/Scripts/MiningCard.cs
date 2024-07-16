using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MiningCard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI cardNameLabel;
    [SerializeField]
    private TextMeshProUGUI cardTotalProfitLabel;
    [SerializeField]
    private TextMeshProUGUI cardProfitPerHourLabel;
    [SerializeField]
    private TextMeshProUGUI cardCostLabel;
    [SerializeField]
    private TextMeshProUGUI cardLevelLabel;
    [SerializeField]    
    private CloudImage cloudImage;
    public void InitCard(Card card)
    {
        cardNameLabel.text = card.name;
        cardProfitPerHourLabel.text = card.profit.profitPerHour.ToString();
        cardTotalProfitLabel.text = card.profit.totalProfit.ToString();
       // cardLevelLabel.text = card.currentLevel.ToString();
        cardCostLabel.text = card.cost.ToString();
        cloudImage.Init(card.image_url);
    }


    public void BuyCard()
    {

    }
}
