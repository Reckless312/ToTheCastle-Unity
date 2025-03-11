using System;
using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
    [SerializeField] private int objectIndex = 0;

    const int PLAYER_OBJECT_INDEX = 0;
    const int CAMERA_OBJECT_INDEX = 1;
    const int GAME_INPUT_OBJECT_INDEX = 2;
    const int SCENE_CONTROLLER_OBJECT_INDEX = 3;
    const int CANVAS_OBJECT_INDEX = 4;
    const int ENEMY_OBJECT_INDEX = 5;

    //Last used index: 5;
    private static GameObject[] persistentObjects = new GameObject[10];
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

    public static PlayerEvents PlayerEvents
    {
        get
        {
            return persistentObjects[PLAYER_OBJECT_INDEX].GetComponent<PlayerEvents>();
        }
    }

    public static ThirdPersonCamera ThirdPersonCamera
    {
        get
        {
            return persistentObjects[CAMERA_OBJECT_INDEX].GetComponent<ThirdPersonCamera>();
        }
    }

    public static GameInput GameInput
    {
        get
        {
            return persistentObjects[GAME_INPUT_OBJECT_INDEX].GetComponent<GameInput>();
        }
    }

    public static SceneController SceneController
    {
        get
        {
            return persistentObjects[SCENE_CONTROLLER_OBJECT_INDEX].GetComponent<SceneController>();
        }
    }

    public static GameObject Canvas
    {
        get
        {
            return persistentObjects[CANVAS_OBJECT_INDEX];
        }
    }

    public static EnemyEvents EnemyEvents
    {
        get
        {
            return persistentObjects[ENEMY_OBJECT_INDEX].GetComponent<EnemyEvents>();
        }
    }
}
