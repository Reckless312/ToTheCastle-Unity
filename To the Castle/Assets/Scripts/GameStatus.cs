using UnityEngine;

public class GameStatus : MonoBehaviour
{
    private float timeToRestart = 5f;

    private void Awake()
    {
        ClearScreen();
    }

    public void ShowStatus()
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
