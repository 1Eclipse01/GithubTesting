using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject explosion;

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
        Destroy(gameObject);
        Instantiate(explosion, transform.position, transform.rotation);
    }
}
