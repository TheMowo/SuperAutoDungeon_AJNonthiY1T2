using System.Collections;
using UnityEngine;

public class Delay : MonoBehaviour
{
    public delegate void delayDelegate(float delayTime);
    delayDelegate delayTimeDelegate;

    void Start()
    {
        delayTimeDelegate += TheYo;
        
        StartCoroutine(DelayTime(1, delayTimeDelegate));
    }

    public IEnumerator DelayTime(float delayTime, delayDelegate delayTimeDelegate)
    {
        yield return new WaitForSeconds(delayTime);
        delayTimeDelegate.Invoke(1);
    }

    void TheYo(float num)
    {
        Debug.Log("test");
    }
    
}
