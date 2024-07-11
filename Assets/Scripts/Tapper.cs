using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tapper : MonoBehaviour
{
    private Quaternion defaultRotation;
    private float rotateWhenTapped = 5f;
    [SerializeField]
    private GameObject rabbitBG;
    private int defaultCoinIncrease = 8;
    private const string coinIncreasePrefsKey = "CoinIncrease";
    private int currentCoinIncrease;
    [SerializeField]
    private GameObject labelRoot;
    [SerializeField]
    private FadingLabel scoreEarnedLabel;
    //when tapped spawn from a pool of disabled labels
    private void Start()
    {
        defaultRotation = transform.rotation;
        if(PlayerPrefs.HasKey(coinIncreasePrefsKey))
            currentCoinIncrease = PlayerPrefs.GetInt(coinIncreasePrefsKey);
        else
        {
            currentCoinIncrease = defaultCoinIncrease;
            PlayerPrefs.SetInt(coinIncreasePrefsKey, defaultCoinIncrease);
            PlayerPrefs.Save();
        }
    }

    public void OnTapped()
    {
        //TODO:use Object pooling
        Vector3 position = Input.mousePosition;
        FadingLabel fadingLabel =  Instantiate(scoreEarnedLabel, position,Quaternion.identity,labelRoot.transform);
        fadingLabel.Init(currentCoinIncrease);
        StartCoroutine(RotateRabbit());
        UserDataHandler.Instance.IncreaseCoins(currentCoinIncrease);
        
    }

    private IEnumerator RotateRabbit()
    {
        rabbitBG.transform.Rotate(new Vector3(rotateWhenTapped, 0, 0));
        yield return new WaitForSeconds(0.5f);
        rabbitBG.transform.rotation = defaultRotation;
    }

}
