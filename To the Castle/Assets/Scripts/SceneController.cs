using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private PlayerEvents playerEvents;
    private GameObject Canvas;

    public void Start()
    {
        Canvas = DoNotDestroy.Canvas;
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(sceneIndex == 0)
        {
            Canvas.SetActive(false);
        }
    }

    public void ChangeArea()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        playerEvents = DoNotDestroy.PlayerEvents;
        Canvas = DoNotDestroy.Canvas;
        if (sceneIndex == 0)
        {
            SceneManager.LoadScene(sceneIndex + 1);
            playerEvents.HandleChangeAnimatorController(sceneIndex + 1);
            Canvas.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(sceneIndex - 1);
            playerEvents.HandleChangeAnimatorController(sceneIndex - 1);
            Canvas.SetActive(false);
        }
    }
}
