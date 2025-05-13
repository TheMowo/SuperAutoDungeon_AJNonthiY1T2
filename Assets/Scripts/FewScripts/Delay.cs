using System;
using System.Collections;
using UnityEngine;

public class Delay : MonoBehaviour
{
    private static Delay _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Run(float delaySeconds, Action callback)
    {
        if (_instance != null)
        {
            _instance.StartCoroutine(_instance.DelayCoroutine(delaySeconds, callback));
        }
        else
        {
            Debug.LogError("Delay instance not found in the scene.");
        }
    }

    private IEnumerator DelayCoroutine(float delaySeconds, Action callback)
    {
        yield return new WaitForSeconds(delaySeconds);
        callback?.Invoke();
    }
    
}
