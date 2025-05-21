using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreen : MonoBehaviour
{
    CheckPlayerLose _checkPlayerLose;
    [SerializeField] GameObject loseScreen;
    private void Start()
    {
        _checkPlayerLose = FindFirstObjectByType<CheckPlayerLose>();
    }
    private void Update()
    {
        if (_checkPlayerLose.isOver)
        {
            loseScreen.SetActive(true);
        }
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
