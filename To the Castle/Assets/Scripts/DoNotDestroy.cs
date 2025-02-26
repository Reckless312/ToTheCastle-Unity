using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
    [SerializeField] private int objectIndex = 0;

    private static GameObject[] persistentObjects = new GameObject[5];

    private void Awake()
    {
        if(persistentObjects[objectIndex] == null)
        {
            persistentObjects[objectIndex] = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else if (persistentObjects[objectIndex] != gameObject)
        {
            Destroy(gameObject);
        }
    }
}
