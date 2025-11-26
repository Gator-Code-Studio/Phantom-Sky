using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{

    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2f;

    private float journeyLength;
    private float startTime;

    void Start()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(pointA.position, pointB.position);
    }

    void Update()
    {
        float distanceCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distanceCovered / journeyLength;
        float pingPongValue = Mathf.PingPong(fractionOfJourney, 1f);

        transform.position = Vector3.Lerp(pointA.position, pointB.position, pingPongValue);
    }








}
