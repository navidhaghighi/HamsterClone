using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WelcomeBackMessage : MonoBehaviour,IObserver
{
    [SerializeField]
    private TextMeshProUGUI inactiveCoinsLabel;
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
            UserDataHandler userData = (UserDataHandler)subject;
            inactiveCoinsLabel.text = userData.inactiveCoins.ToString();
            
        }
    }


}
