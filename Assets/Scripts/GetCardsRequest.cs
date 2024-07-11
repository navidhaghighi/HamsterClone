using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCardsRequest : MonoBehaviour
{


}
// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
[System.Serializable]
public class Card
{
    public string name;
    public int initialProfit;
    public Reward reward;
}
[System.Serializable]

public class Reward
{
    public int coins;
}   
[System.Serializable]
public class GetCardsResponse
{
    public List<Card> cards;
}

