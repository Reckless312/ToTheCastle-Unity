using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUit : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        startButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.CastleEntrance);
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}

