using UnityEngine;

public class MainGate : MonoBehaviour
{
    [SerializeField] private SceneController sceneController;

    public void Interact()
    {
        sceneController.ChangeArea();
    }
}
