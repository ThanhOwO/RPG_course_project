using UnityEngine;

public class TeleportCameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Vector3 targetPosition;

    private void Start()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.unscaledDeltaTime);
    }

    public void FocusOnSavePoint(Vector2 savePointPosition)
    {
        targetPosition = new Vector3(savePointPosition.x, savePointPosition.y, transform.position.z);
    }
}
