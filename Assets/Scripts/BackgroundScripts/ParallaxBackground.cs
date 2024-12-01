using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam;
    [SerializeField] public float ParallaxEffect;
    private float xPosition;

    void Start()
    {
        cam = GameObject.Find("Main Camera");

        xPosition = transform.position.x;
    }

    void Update()
    {
        float distanceToMove = cam.transform.position.x * ParallaxEffect;

        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);
    }
}
