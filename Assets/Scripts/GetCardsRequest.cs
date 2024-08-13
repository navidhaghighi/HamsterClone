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
    public int initial_profit;
    public string image_url;
}
[System.Serializable]
public class UserCard
{
    public int card_id;
    public int current_level;
    public int profit;

}
[System.Serializable]
public class GetCardsResponse:HTTPResponse
{
    public List<Card> cards;
}

