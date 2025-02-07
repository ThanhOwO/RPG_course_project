using System.Collections;
using Cinemachine;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineConfiner2D confiner;
    public Transform player;
    public RoomFadeCanvas roomFadeCanvas;

    private void Awake() 
    {
        if (instance != null && instance != this) 
            Destroy(instance.gameObject);
        else
            instance = this;

        if (virtualCamera != null)
        {
            confiner = virtualCamera.GetComponent<CinemachineConfiner2D>();
        }
    }

    public void MovePlayerToRoom(Room newRoom, Vector2 newPosition, bool useFade = true)
    {
        StartCoroutine(TransitionToRoom(newRoom, newPosition, useFade));
    }

    public void UpdateConfiner(string _roomId)
    {
        Room[] rooms = FindObjectsByType<Room>(FindObjectsSortMode.None);

        foreach(Room room in rooms)
        {
            if(room.roomID == _roomId)
            {
                PolygonCollider2D bounds = room.GetComponentInChildren<PolygonCollider2D>();
                if (bounds!= null && confiner!= null)
                {
                    confiner.m_BoundingShape2D = bounds;
                    confiner.InvalidateCache();
                }
            }
            break;
        }
    }
    
    private IEnumerator TransitionToRoom(Room newRoom, Vector2 newPosition, bool useFade)
    {
        if (useFade && roomFadeCanvas != null)
        {
            roomFadeCanvas.FadeOut();
            yield return new WaitUntil(() => roomFadeCanvas.isFadingOut == false);
        }

        player.position = newPosition;
        virtualCamera.Follow = player;

        PolygonCollider2D newBounds = newRoom.GetComponentInChildren<PolygonCollider2D>();
        if (newBounds != null && confiner != null)
        {
            confiner.m_BoundingShape2D = newBounds;
            confiner.InvalidateCache();
        }

        yield return new WaitForSeconds(0.3f);

        if (useFade && roomFadeCanvas != null)
        {
            roomFadeCanvas.FadeIn();
        }
    }
}
