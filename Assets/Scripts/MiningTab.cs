using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningTab : MonoBehaviour,IObserver
{
    private List<MiningCard> miningCards = new List<MiningCard>(); 
    [SerializeField]
    private Transform miningCardsParent;
    [SerializeField]
    private MiningCard cardPrefab;
    // Start is called before the first frame update
    void Start()
    {
        MiningDataHandler.Instance.Attach(this);
    }
    [ContextMenu("DelteAll")]
    public void DelteAll()
    {
        PlayerPrefs.DeleteAll();

        Debug.LogWarning("Deleted ");
    }

    private void OnDestroy()
    {
        MiningDataHandler.Instance.Detach(this);
    }

    public void UpdateObserver(ISubject subject)
    {
        //TODO:instantiate cards only if there is a new one.
        if (subject is MiningDataHandler)
        {
            MiningDataHandler miningData = (MiningDataHandler)subject;
            RefreshCards( miningData.GetCards());
            RefreshUserCards(miningData.GetUserCards());
        }
    }
    //update cards with data about user's current card's levels etc...
    private void RefreshUserCards(List<UserCard> userCards)
    {
        foreach (var card in miningCards)
        {
            foreach (var userCard in userCards)
            {
                if(card.GetCardId() == userCard.cardId)
                    card.SetUserData(userCard);
            }
        }

    }

    private void RefreshCards(List<Card> cards)
    {
        foreach (Transform child in miningCardsParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Card card in cards)
        {
            var instantiatedCard =   Instantiate( cardPrefab,miningCardsParent );
            instantiatedCard.InitCard(card);
            miningCards.Add(instantiatedCard);
        }

    }

}
