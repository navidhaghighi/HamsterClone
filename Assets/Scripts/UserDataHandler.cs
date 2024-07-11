using System.Collections.Generic;

public class UserDataHandler : ISubject
{
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


    public int GetProfitPerHour()
    {
        return profitPerHour;
    }

    public void Attach(IObserver observer)
    {
        observers.Add(observer);
    }

    public Rank GetCurrentRank()
    {

        return currentRank;

    }

    public int GetCurrentRankIndex()
    {

        return ranks.IndexOf(currentRank);

    }



    public int GetRanksCount()
    {
        return ranks.Count;
    }

    public void Detach(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void IncreaseProfit(int profit)
    {
        profitPerHour+= profit;
        Notify();
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
}
[System.Serializable]
public class Rank
{
    public string rankName;
    public int maximumCoin;
}
