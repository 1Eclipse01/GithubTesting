using UnityEngine;

public class GameManagerTask8 : MonoBehaviour
{
    [SerializeField] float speed = 20;
    [SerializeField] Vector3 distanceToTravel = new Vector3(-60, 0, 0);

    float waypointTimer = 0;
    private float travelTime = 6;


    void Start()
    {
    }

    void Update()
    {
        waypointTimer = Mathf.Repeat(waypointTimer+Time.deltaTime, travelTime);
        bool goingLeft = (waypointTimer < travelTime / 2);

    }

    private void MoveToWayPoint(GameObject enemy, Vector3 waypoint)
    {
        if (enemy == null) return;
        Vector3 movefrom = enemy.transform.position;
        enemy.transform.position = Vector3.MoveTowards(movefrom, waypoint, speed * Time.deltaTime);

        float targetY = 180;
        if (movefrom.x > waypoint.x) targetY = -150;
        if (movefrom.x < waypoint.x) targetY = 150;
        float targetX = enemy.transform.rotation.eulerAngles.x;
        float targetZ = enemy.transform.rotation.eulerAngles.z;
        Quaternion targetRot = Quaternion.Euler(targetX, targetY, targetZ);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRot, 0.1f);
    }
}
