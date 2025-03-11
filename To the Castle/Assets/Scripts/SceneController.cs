using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private PlayerEvents playerEvents;

    public void ChangeArea()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        playerEvents = DoNotDestroy.PlayerEvents;
        if (sceneIndex == 0)
        {
            SceneManager.LoadScene(sceneIndex + 1);
            playerEvents.HandleChangeAnimatorController(sceneIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(sceneIndex - 1);
            playerEvents.HandleChangeAnimatorController(sceneIndex - 1);
        }
    }
}
