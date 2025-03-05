using UnityEngine;
using UnityEngine.Events;

public class TestAudioEnemy : MonoBehaviour
{
    [SerializeField] private UnityEvent _onDeath;

    void EnemyDies()
    {
        _onDeath.Invoke();
    }
}
