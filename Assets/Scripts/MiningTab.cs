using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningTab : MonoBehaviour,IObserver
{
    [SerializeField]
    private Transform miningCardsParent;
    [SerializeField]
    private MiningCard cardPrefab;
    // Start is called before the first frame update
    void Start()
    {
        MiningDataHandler.Instance.Attach(this);
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
        }
    }

    private void RefreshCards(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            var instantiatedCard =   Instantiate( cardPrefab,miningCardsParent );
            instantiatedCard.InitCard(card);
        }

    }

}
