using UnityEngine;

public class MapCameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float zoomSpeed = 2f;
    public float minZoom = 60f;
    public float maxZoom = 100f;
    [SerializeField] private Camera mapCamera;
    private Vector3 defaultPosition;
    private float defaultZoom;
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;
    public static bool isTeleportMap = false;

    
    void Start()
    {
        defaultPosition = transform.position;
        defaultZoom = mapCamera.orthographicSize;
    }

    void LateUpdate()
    {
        if(Time.timeScale == 0)
        {
            HandleMovement();
            HandleZoom();
        }
    }

    void HandleMovement()
    {
        if(isTeleportMap) return;

        float moveX = Input.GetAxisRaw("CameraHorizontal");
        float moveY = Input.GetAxisRaw("CameraVertical");

        Vector3 move = new Vector3(moveX, moveY, 0) * moveSpeed * Time.unscaledDeltaTime;
        transform.position += move;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x),
            Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y),
            transform.position.z
        );

    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            mapCamera.orthographicSize -= scroll * zoomSpeed;
            mapCamera.orthographicSize = Mathf.Clamp(mapCamera.orthographicSize, minZoom, maxZoom);
        }
    }

    public void ResetCamera()
    {
        if(Camera.main != null)
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
        mapCamera.orthographicSize = defaultZoom;
    }
}
