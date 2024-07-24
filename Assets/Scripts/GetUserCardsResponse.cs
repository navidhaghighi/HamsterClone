using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MyArray
{
    public int id;
    public int user_id;
    public int card_id;
    public int current_level;
}
[System.Serializable]
public class GetUserCardsResponse
{
    public List<Card> cards;
}

