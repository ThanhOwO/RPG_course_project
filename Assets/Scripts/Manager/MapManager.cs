using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    private HashSet<string> discoveredRooms = new HashSet<string>();
    private void Awake() 
    {
        if (instance != null && instance != this) 
            Destroy(instance.gameObject);
        else
            instance = this;

    }

    public void DiscoverRoom(string roomID)
    {
        if (!discoveredRooms.Contains(roomID))
        {
            discoveredRooms.Add(roomID);
            Room room = FindRoomByID(roomID);
            if (room != null)
                room.DiscoverRoom();
        }
    }

    private Room FindRoomByID(string roomID)
    {
        Room[] allRooms = FindObjectsByType<Room>(FindObjectsSortMode.None);
        foreach (Room room in allRooms)
        {
            if (room.roomID == roomID)
                return room;
        }
        return null;
    }

    public List<string> GetDiscoveredRooms()
    {
        return new List<string>(discoveredRooms);
    }

    public void LoadDiscoveredRooms(List<string> savedRooms)
    {
        discoveredRooms = new HashSet<string>(savedRooms);
        Room[] allRooms = FindObjectsByType<Room>(FindObjectsSortMode.None);
        foreach (Room room in allRooms)
        {
            if (discoveredRooms.Contains(room.roomID))
                room.DiscoverRoom();
        }
    }

    public bool IsRoomDiscovered(string roomID)
    {
        return discoveredRooms.Contains(roomID);
    }

    public void InitializeDiscoveredRooms(string initialRoomID)
    {
        discoveredRooms.Clear();
        discoveredRooms.Add(initialRoomID);
        Room room = FindRoomByID(initialRoomID);
        if (room != null)
            room.DiscoverRoom();
    }
}
