using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerControllerTask8 : MonoBehaviour
{
    [SerializeField] private Key rightKey = Key.D;
    [SerializeField] private Key leftKey = Key.A;
    [SerializeField] private Key shootKey = Key.Space;
    [SerializeField] Rigidbody body;
    [SerializeField] private float accelerationForce=100;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject projectile;

    void Start()
    {
        body = gameObject.GetComponentInChildren<Rigidbody>();
    }

    void Update()
    {
        float xInput = Keyboard.current[rightKey].value - Keyboard.current[leftKey].value;
        body.linearVelocity += new Vector3(xInput * Time.deltaTime * accelerationForce, 0, 0);
        float x = Mathf.Clamp(transform.position.x, -80, 80);
        transform.position = new Vector3(x, transform.position.y, 0);
        float targetRotation = xInput * -45;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, targetRotation,  0), 0.1f);

        if (Keyboard.current[shootKey].wasPressedThisFrame)
        {
            var shot = Instantiate(projectile, firePoint.position, firePoint.rotation);
            shot.GetComponent<Rigidbody>().linearVelocity = new Vector3(0, 150, 0);
            Destroy(shot, 2);
        }
    }
}
