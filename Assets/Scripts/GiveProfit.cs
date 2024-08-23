using System;
using System.Collections;
using UnityEngine;

public class GiveProfit : MonoBehaviour,IObserver
{
    private float currentProfitPerHour;
    private const float coinGiveInterval=1f;
    // Start is called before the first frame update
    void Start()
    {
        UserDataHandler.Instance.Attach(this);
        StartCoroutine(GiveCoins());
    }


    private void OnDestroy()
    {
        UserDataHandler.Instance.Detach(this);
    }

    public IEnumerator GiveCoins()
    {
        while (true)
        {
            int profitForInterval =(int)Math.Round( (currentProfitPerHour/3600)*coinGiveInterval);
            yield return new WaitForSeconds(coinGiveInterval);
            UserDataHandler.Instance.IncreaseCoins(profitForInterval);
        }

    }

    public void UpdateObserver(ISubject subject)
    {

        if(subject is UserDataHandler)
        {
            UserDataHandler userData = (UserDataHandler)subject;
            currentProfitPerHour = userData.currentUser.profit;
        }
    }
}
