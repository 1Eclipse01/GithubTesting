using UnityEngine;

public class TargetCollision : MonoBehaviour
{
    private bool beenHit = false;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name != "Floor" && !beenHit)
        {
            beenHit = true;
            FindAnyObjectByType<PlayerControllerTask8B>().TargetShot(gameObject);
        }
    }
}
