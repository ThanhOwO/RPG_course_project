using UnityEngine;

public class Room : MonoBehaviour
{
    public string roomID;

    private void Awake()
    {
        if (string.IsNullOrEmpty(roomID))
        {
            roomID = gameObject.name;
        }
    }

}
