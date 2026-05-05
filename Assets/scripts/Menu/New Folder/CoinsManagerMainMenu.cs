using System;
using UnityEngine;

public class CoinsManagerMainMenu : MonoBehaviour
{
    public static CoinsManagerMainMenu instance;
    public int Coins {get; private set;} = 0;
    public event Action<int> OnCoinsChanged;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Coins = PlayerPrefs.GetInt(PlayerPrefsKeys.PlayerCoins);
    }
    public void AddCoins(int value)
    {
        if (value > 0) { 
            Coins += value;
            OnCoinsChanged?.Invoke(Coins);
        }
    }
    public bool TrySpendCoins(int value)
    {
        return Coins >= value;
    }
    public void SpendCoins(int value)
    {
        if (TrySpendCoins(value))
        {
            Coins -= value;
            OnCoinsChanged?.Invoke(Coins);
        }
        else
        {
            Debug.Log("Not enough coins to purchase");
        }
    }
}
