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
    public int id;
    public string name;
    public int cost;
    public Profit profit;
    public string image_url;
}
[System.Serializable]
public class UserCard
{
    public int cardId;
    public int currentLevel;

}

[System.Serializable]

public class Profit
{
    public int profitPerHour;
    public int totalProfit;
}   
[System.Serializable]
public class GetCardsResponse
{
    public List<Card> cards;
}

