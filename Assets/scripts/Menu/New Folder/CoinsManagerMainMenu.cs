using System;
using UnityEngine;
using System.Collections;

public class CoinsManagerMainMenu : Savable
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
    public override void Load(DataSave dataSave)
    {
        if (dataSave != null)
        {
            Coins = dataSave.Coins;
            OnCoinsChanged?.Invoke(Coins);
        }
    }
    public override void Save(DataSave dataSave)
    {
        if (dataSave != null)
        {
            dataSave.SetCoins(Coins);
        }
    }
}
