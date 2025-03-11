using UnityEngine;

public class SceneController : MonoBehaviour
{
    private PlayerEvents playerEvents;
    private GameObject Canvas;

    public void Start()
    {
        Canvas = DoNotDestroy.Canvas;
        if(Loader.GetScene() == Loader.Scene.CastleEntrance)
        {
            Canvas.SetActive(false);
        }
    }

    public void ChangeArea()
    {
        playerEvents = DoNotDestroy.PlayerEvents;
        Canvas = DoNotDestroy.Canvas;

        if (Loader.GetScene() == Loader.Scene.CastleEntrance)
        {
            Loader.Load(Loader.Scene.FirstFloor);
            playerEvents.HandleChangeAnimatorController(Loader.Scene.FirstFloor);
            Canvas.SetActive(true);
        }
        else if(Loader.GetScene() == Loader.Scene.FirstFloor)
        {
            Loader.Load(Loader.Scene.CastleEntrance);
            playerEvents.HandleChangeAnimatorController(Loader.Scene.CastleEntrance);
            Canvas.SetActive(false);
        }
    }
}
