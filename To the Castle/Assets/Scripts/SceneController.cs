using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void ChangeArea()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(sceneIndex == 0)
        {
            SceneManager.LoadScene(sceneIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(sceneIndex - 1);
        }
    }
}
