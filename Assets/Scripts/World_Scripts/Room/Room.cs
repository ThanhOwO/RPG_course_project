using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    public string roomID;
    private TilemapRenderer tilemapRenderer;

    private void Awake()
    {
        if (string.IsNullOrEmpty(roomID))
            roomID = gameObject.name;

        tilemapRenderer = GetComponentInChildren<TilemapRenderer>();

        if (tilemapRenderer != null)
            tilemapRenderer.enabled = false;
    }

    private void Start()
    {
        if (MapManager.instance.IsRoomDiscovered(roomID))
            DiscoverRoom();
        else if (IsInitialSpawnRoom())
            MapManager.instance.DiscoverRoom(roomID);
    }

    public void DiscoverRoom()
    {
        if (tilemapRenderer != null)
            tilemapRenderer.enabled = true;
    }

    private bool IsInitialSpawnRoom()
    {
        return roomID == "Room";
    }

}
