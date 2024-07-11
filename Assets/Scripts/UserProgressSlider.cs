using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Slider))]
public class UserProgressSlider :MonoBehaviour,  IObserver
{
    [SerializeField]
    private TextMeshProUGUI totalRanksLabel;
    
    [SerializeField]
    private TextMeshProUGUI rankLabel;
    private Slider progressSlider;
    private void Start()
    {
        progressSlider = GetComponent<Slider>();
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
            var userData = (UserDataHandler)subject;
            totalRanksLabel.text = (userData.GetCurrentRankIndex()+1) + " / " + userData.GetRanksCount();
            rankLabel.text = userData.GetCurrentRank().rankName.ToString();
            progressSlider.maxValue = userData.GetCurrentRank().maximumCoin;
            progressSlider.value = userData.GetCoinAmount();
        }
    }
}
