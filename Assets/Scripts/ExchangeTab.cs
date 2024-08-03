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
        UserDataHandler.Instance.Attach(this);
    }

    private void OnDestroy()
    {
        UserDataHandler.Instance.Detach(this);
    }
    public void UpdateObserver(ISubject subject)
    {
        if (subject is UserDataHandler)
        {
            UserDataHandler dataHandler = (UserDataHandler)subject;
            coinsToLevelUpLabel.text = dataHandler.GetCoinsToLevelUp().ToString();
            currentProfitLabel.text = dataHandler.GetProfitPerHour().ToString();
            tapToEarnLabel.text = dataHandler.currentUser.earn_per_tap.ToString();
        }
    }

}
