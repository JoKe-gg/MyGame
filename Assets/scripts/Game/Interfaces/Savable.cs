using System.Collections;
using UnityEngine;

public abstract class Savable : MonoBehaviour
{
    public abstract void Load(DataSave saveData);
    public abstract void Save(DataSave saveData);
    private Coroutine _coroutine;
    protected virtual void OnEnable()
    {
        if (SaveManager.instance != null)
        {
            SaveManager.instance.Register(this);
        }
        else
        {
            _coroutine = StartCoroutine(WaitToRegister());
        }
    }
    protected virtual void OnDisable()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
        if (SaveManager.instance != null)
        {
            SaveManager.instance.Unregister(this);
        }
    }
    private IEnumerator WaitToRegister()
    {
        
        while (SaveManager.instance == null)
        {
            yield return null;
        }
        SaveManager.instance.Register(this);
       _coroutine = null;

    }
}
