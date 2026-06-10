using UnityEngine;

public class CoinandLivesCoin : GenericCoin
{
    public int liveAmount = 1;
    public int coinAmount = 1;
    
    public override void CollectMyself()
    {
        gameGui.AddLives(liveAmount);
        gameGui.AddGoldenCoins(coinAmount);
    }
}
