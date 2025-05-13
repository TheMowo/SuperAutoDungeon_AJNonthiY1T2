using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton_NewGame : MonoBehaviour
{
    // Check if a save exists
    // If it does, prompt to confirm before proceeding
    // Resets save file
    // Starts new game, transitioning into Scene 1
    private bool hasSave;

    public void NewGame_ButtonPressed()
    {
        if (hasSave)
        {
            //prompt confirmation
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }
}
