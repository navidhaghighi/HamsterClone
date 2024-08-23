using System.Collections.Generic;
using UnityEngine;

public class UserDataHandler : ISubject
{
    public int inactiveCoins;
    public User currentUser;
    private int userId;
    private List<Rank> ranks = new List<Rank>()
    {
        new Rank()
        {
            rankName = "Silver", maximumCoin = 5000
        },
        new Rank()
        {
            rankName = "Gold", maximumCoin = 10000
        },

        new Rank()
        {
            rankName = "Diamond", maximumCoin = 20000
        },

    };
    private Rank currentRank;
    private int coinsToLevelUp;
    private int profitPerHour;
    private int coinsAmount;
    private List<IObserver> observers = new List<IObserver>();
    #region singleton
    private UserDataHandler()
    {
        foreach (var rank in ranks)
        {
            if (coinsAmount < rank.maximumCoin)
            {
                currentRank = rank;
                break;
            }
        }
        RefreshUserData();

    }

    private static UserDataHandler instance;
    public static UserDataHandler Instance
    {
        get
        {
            if (instance == null)
                instance = new UserDataHandler();
            return instance;
        }
    }
    #endregion singleton

    public void RefreshUserData()
    {
        int userId = PlayerPrefs.GetInt("UserId");
        if (userId == 0)
        {
            SendCreateUserRequest();
        }
        else
        {
            SendGetUserDataRequest(userId);
        }
    }

    public void SendGetUserDataRequest(int userId)
    {
        var req = new HttpRequest<LoginResponse>();
        ContextManager.Instance.StartCoroutine(req.SendRequest(ServerConfig.baseURL + "/login", (response) =>
        {
            SaveUser(response.user);
            inactiveCoins = response.coinsReceived;
            Notify();
        }, "userId=" + userId.ToString()));
    }

    private void SaveUser(User user)
    {
        currentUser = user;
        userId = user.id;
        coinsAmount = user.coin_balance;
        coinsToLevelUp = user.coins_to_level_up;
        PlayerPrefs.SetInt("UserId", user.id);
        PlayerPrefs.SetInt("UserCoins", user.coin_balance);
        PlayerPrefs.SetInt("UserProfit", user.profit);
        PlayerPrefs.SetString("UserName", user.name);
    }


    private void SendCreateUserRequest()
    {
        var req = new HttpRequest<NewUserResponse>();
        ContextManager.Instance.StartCoroutine(req.SendRequest(ServerConfig.baseURL + "/createNewUser", (response) =>
        {
            SaveUser(response.user);
            Notify();
        }));
    }

    public void Attach(IObserver observer)
    {
        observers.Add(observer);
        InitializeObserver(observer);
    }

    public Rank GetCurrentRank()
    {

        return currentRank;

    }

    public int GetCurrentRankIndex()
    {

        return ranks.IndexOf(currentRank);

    }


    public int GetUserId()
    {
        return userId;
    }

    public int GetRanksCount()
    {
        return ranks.Count;
    }

    public void Detach(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void Tapped(MonoBehaviour context)
    {
        Debug.LogWarning("Tapped request id is " + userId);
        var req = new HttpRequest<User>();
        ContextManager.Instance.StartCoroutine(  req.SendRequest(ServerConfig.baseURL + "/tapped", (response) =>
        {
            SaveUser(response);
            Notify();
        },"userId="+userId));
    }

    public void IncreaseCoins(int amount)
    {
        coinsAmount += amount;
        if (coinsAmount > currentRank.maximumCoin)
            GoNextRank();
        Notify();
    }


    private void GoNextRank()
    {
        //find index of current rank 
        int currentRankIndex = ranks.IndexOf(currentRank);
        int nextRank = currentRankIndex + 1;
        if ((nextRank) < ranks.Count)
        {
            currentRank = ranks[nextRank];
        }
    }

    public int GetCoinAmount()
    {
        return coinsAmount;
    }

    public void Notify()
    {
        foreach (var observer in observers)
        {
            observer.UpdateObserver(this);
        }
    }

    public void InitializeObserver(IObserver observer)
    {
        Notify();
    }
}
[System.Serializable]
public class Rank
{
    public string rankName;
    public int maximumCoin;
}
[System.Serializable]
public class User
{
    public int id;
    public string name;
    public int profit;
    public int coin_balance;
    public int earn_per_tap;
    public int coins_to_level_up;
}
