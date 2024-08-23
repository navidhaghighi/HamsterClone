using TMPro;
using UnityEngine;

public class ExchangeTab : MonoBehaviour, IObserver
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
            coinsToLevelUpLabel.text = dataHandler.currentUser.coins_to_level_up.ToString();
            currentProfitLabel.text = dataHandler.currentUser.profit.ToString();
            if (dataHandler.currentUser != null)
                tapToEarnLabel.text = dataHandler.currentUser.earn_per_tap.ToString();
        }
    }

}
