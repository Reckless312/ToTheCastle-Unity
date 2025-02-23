using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
