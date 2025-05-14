using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public ItemSaveSystem ISS;

    public void LoadNextScene()
    {
        ISS = FindFirstObjectByType<ItemSaveSystem>();
        ISS.ItemSaveData();
        SceneManager.LoadScene("Stage2");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
