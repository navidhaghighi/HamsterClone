using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TextMeshProUGUI))]
public class CurrentProfitLabel : MonoBehaviour,IObserver
{
    private TextMeshProUGUI profitLabel;
    // Start is called before the first frame update
    void Start()
    {
        profitLabel = GetComponent<TextMeshProUGUI>();
        UserDataHandler.Instance.Attach(this);
    }

    private void OnDestroy()
    {
        UserDataHandler.Instance.Detach(this);
    }

    public void UpdateObserver(ISubject subject)
    {
        if(subject is UserDataHandler)
        {
            UserDataHandler userData = (UserDataHandler)subject;
            profitLabel.text = userData.GetProfitPerHour().ToString();
        }
    }
}
