using UnityEngine;

public class GameOver : MonoBehaviour
{
    private void Awake()
    {
        ClearScreen();
    }

    public void ShowGameOver()
    {
        gameObject.SetActive(true);
    }

    public void ClearScreen()
    {
        gameObject.SetActive(false);
    }
}
