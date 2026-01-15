using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float length, startPos;
    public GameObject cam;
    public float parallaxFactor;

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float dist = (cam.transform.position.x * parallaxFactor);
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
    }
}