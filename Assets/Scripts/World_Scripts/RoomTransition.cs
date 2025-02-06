using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    public Room targetRoom;
    public Transform targetPosition;
    public Vector2 positionOffset;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
        {
            if (targetRoom != null && targetRoom.roomID != null)
            {
                Vector2 newPos = (Vector2)targetPosition.position + positionOffset;
                RoomManager.instance.MovePlayerToRoom(targetRoom, newPos);
            }
        }
    }
}
