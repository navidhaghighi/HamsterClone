using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MiningCard : MonoBehaviour,IObserver
{
    private int cardLevel;
    private int userId;
    private int cardId;
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

    private void Awake()
    {
        UserDataHandler.Instance.Attach(this);
        UserMiningDataHandler.Instance.Attach(this);
    }

    private void OnDestroy()
    {
        UserDataHandler.Instance.Detach(this);
        UserMiningDataHandler.Instance.Detach(this);
    }

    public void SetUserData(UserCard userCard)
    {
        cardLevelLabel.text ="LVL"+ userCard.current_level.ToString();
        cardLevel = userCard.current_level;
        cardProfitPerHourLabel.text = userCard.profit.ToString();
        cardCostLabel.text = userCard.cost.ToString();
    }

    public void InitCard(Card card)
    {
        cardId = card.id;
        cardNameLabel.text = card.name;
        cardProfitPerHourLabel.text = card.profit.ToString();
       // cardLevelLabel.text = card.currentLevel.ToString();
        cardCostLabel.text = card.cost.ToString();
        cloudImage.Init(card.image_url);
    }

    public int GetCardId() { return cardId; }   


    public void BuyCard()
    {
        if(cardLevel==0)
            UserMiningDataHandler.Instance.BuyCard(cardId,userId);
        else
            UserMiningDataHandler.Instance.UpgradeCard(cardId, userId);

    }

    public void UpdateObserver(ISubject subject)
    {
        if (subject is UserDataHandler)
        {
            UserDataHandler dataHandler = (UserDataHandler)subject;
            userId = dataHandler.GetUserId();
        }

    }
}
