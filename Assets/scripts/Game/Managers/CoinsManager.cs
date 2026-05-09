using System;
using UnityEngine;
using System.Collections;
public class CoinsManager : MonoBehaviour, ISavable
{
    public static CoinsManager instance;
    public int Coins { get; private set;}
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
    private void OnEnable()
    {
        if (SaveManager.instance != null)
        {
            SaveManager.instance.Register(this);
        }
        else
        {
            Debug.Log($"SAVE MANAGER IS NOT READY");
            StartCoroutine(WaitToRegister());
        }
    }
    private void OnDisable()
    {
        if (SaveManager.instance != null)
        {
            SaveManager.instance.Unregister(this);
        }
    }
    private IEnumerator WaitToRegister()
    {
        yield return new WaitForEndOfFrame();
        if (SaveManager.instance != null)
        {
            SaveManager.instance.Register(this);
        }
        else
        {
            Debug.Log($"SAVE MANAGER IS NOT READY IN COROUTINE");
        }
    }
    public void AddCoins(int coins)
    {
        Coins += coins;
        OnCoinsChanged?.Invoke(Coins);

    }
    public bool TrySpendCoins(int requiredAmount)
    {
        if (requiredAmount <= Coins)
        {
            return true;
        }
        else 
        { 
            return false; 
        }
    }
    public void Load(DataSave dataSave)
    {
        return;
    }
    public void Save(DataSave dataSave)
    {
        if (dataSave != null)
        {
            dataSave.SetCoins(dataSave.Coins + Coins);
        }
    }
}
