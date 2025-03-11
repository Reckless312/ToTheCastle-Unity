using UnityEngine;

public class GameOver : MonoBehaviour
{
    private float timeToRestart = 3f;

    private void Awake()
    {
        ClearScreen();
    }

    public void ShowGameOver()
    {
        gameObject.SetActive(true);
        Invoke("RestartGame", timeToRestart);
    }

    private void RestartGame()
    {
        ClearScreen();
        DoNotDestroy.Clear();
        Loader.Load(Loader.Scene.MainMenu);
    }

    public void ClearScreen()
    {
        gameObject.SetActive(false);
    }
}
