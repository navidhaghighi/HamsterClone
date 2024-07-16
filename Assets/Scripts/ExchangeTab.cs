using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExchangeTab : MonoBehaviour,IObserver
{
    [SerializeField]
    private TextMeshProUGUI currentProfitLabel;
    [SerializeField]
    private TextMeshProUGUI tapToEarnLabel;
    [SerializeField]
    private TextMeshProUGUI coinsToLevelUpLabel;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void UpdateObserver(ISubject subject)
    {
        if (subject is UserDataHandler)
        {
            UserDataHandler dataHandler = (UserDataHandler)subject;

            currentProfitLabel.text = dataHandler.GetProfitPerHour().ToString();
            tapToEarnLabel.text = dataHandler.currentUser.earn_per_tap.ToString();
        }
    }

}
