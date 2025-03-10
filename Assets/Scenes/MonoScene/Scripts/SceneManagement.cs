using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement Instance; private void Awake() { if (Instance != null) Destroy(this.gameObject); DontDestroyOnLoad(this.gameObject); Instance = this; }
    
    private int _currentSceneIndex;

    private void Start()
    {
        Debug.Log("_currentSceneIndex has been set to current active scene.");
        _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadSpecificLevel(string levelName)
    {
        LoadLevel(levelName, 999);
    }

    public void LoadMainMenu()
    {
        LoadLevel("INSERT MAIN MENU SCENE NAME HERE", 999); //or use index idgaf whatever works
    }

    public void LoadPreviousLevel()
    {
        LoadLevel("NULL", _currentSceneIndex - 1);
    }

    public void LoadNextLevel()
    {
        LoadLevel("NULL", _currentSceneIndex + 1);
    }

    public void RestartLevel()
    {
        LoadLevel("NULL", _currentSceneIndex);
    }

    private void LoadLevel(string levelName, int levelIndex)
    {
        if (levelName != "NULL")
        {
            SceneManager.LoadScene(levelName);
        }
        else if (levelIndex != 999)
        {
            SceneManager.LoadScene(levelIndex);
        }
        return;
    }

    //For Later if I need it (old code):

    //IEnumerator LoadLevel(string levelName, int levelIndex)
    //{
    //    _transition.SetTrigger("Start");

    //    yield return new WaitForSeconds(_transitionTime);

    //    if (levelName != "NULL")
    //    {
    //        SceneManager.LoadScene(levelName);
    //    }
    //    else if (levelIndex != 999)
    //    {
    //        SceneManager.LoadScene(levelIndex);
    //    }

    //}

}
