using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraLookAround : MonoBehaviour
{
    public CinemachineVirtualCamera virtuleCamera;
    private CinemachineFramingTransposer  transposer;
    private Player player;
    private Vector3 defaultOffset;
    private Vector3 targetOffset;
    public float lookAround = 2f;
    public float smoothSpeedX = 5f;
    public float smoothSpeedY = 5f;
    private float initialDeadZoneWidth;
    private float initialDeadZoneHeight;
    private bool isWaiting = false;

    void Start()
    {
       transposer = virtuleCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
       defaultOffset = transposer.m_TrackedObjectOffset;
       targetOffset = defaultOffset;
       initialDeadZoneWidth = transposer.m_DeadZoneWidth;
       initialDeadZoneHeight = transposer.m_DeadZoneHeight;
       player = PlayerManager.instance.player;
    }

    void Update()
    {
        Vector3 moveDir = Vector3.zero;
        bool isInputActive = false;
        
        float camX = Input.GetAxis("CameraHorizontal");
        float camY = Input.GetAxis("CameraVertical");

        if (Mathf.Abs(camX) > 0.1f || Mathf.Abs(camY) > 0.1f)
        {
            isInputActive = true;
        }

        if(player.FacingDir == -1)
            camX = -camX;

        if (isInputActive)
        {
            transposer.m_DeadZoneWidth = 0f;
            transposer.m_DeadZoneHeight = 0f;
            if (isWaiting)
            {
                StopCoroutine("RestoreDeadZone");
                isWaiting = false;
            }
        }
        else
        {
            if (!isWaiting)
            {
                StartCoroutine(RestoreDeadZone());
                isWaiting = true;
            }
        }

        moveDir = new Vector3(camX, camY, 0) * lookAround;

        targetOffset = defaultOffset + moveDir;

        float smoothX = Mathf.Lerp(transposer.m_TrackedObjectOffset.x, targetOffset.x, Time.deltaTime * smoothSpeedX);
        float smoothY = Mathf.Lerp(transposer.m_TrackedObjectOffset.y, targetOffset.y, Time.deltaTime * smoothSpeedY);

        transposer.m_TrackedObjectOffset = new Vector3(smoothX, smoothY, defaultOffset.z);
    }

    private IEnumerator RestoreDeadZone()
    {
        yield return new WaitForSeconds(0.5f);
        transposer.m_DeadZoneWidth = initialDeadZoneWidth;
        transposer.m_DeadZoneHeight = initialDeadZoneHeight;
        isWaiting = false;
    }
}
