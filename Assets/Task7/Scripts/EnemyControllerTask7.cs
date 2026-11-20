using UnityEngine;

public class EnemyControllerTask7 : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] Vector2 initialVelocity = new Vector2(5, 5);
    [SerializeField] float maxY = 4.0f;
    [SerializeField] float minY = -4.0f;
    [SerializeField] float maxX = 8.0f;
    [SerializeField] float minX = -8.0f;
    [SerializeField] GameObject explosionEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = initialVelocity;
    }

    void Update()
    {
        Vector2 newVelocity = rb.linearVelocity;
        if (transform.position.y < minY)
        {
            if(newVelocity.y < 0)
                newVelocity.y *= -1;
            
         
        }
            
        if (transform.position.y > maxY)
        {
            if(newVelocity.y > 0)
                newVelocity.y *= -1;
           
           
        }
            
        if(transform.position.x < minX)
        {
            if(newVelocity.x < 0)
                newVelocity.x *= -1;
           
        }
            
        if(transform.position.x > maxX)
        {
            if(newVelocity.x > 0)
                newVelocity.x *= -1;
            
              
        }
            
        rb.linearVelocity = newVelocity;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        int xPolarity = Random.Range(0, 2) * 2 - 1;
        int yPolarity = Random.Range(0, 2) * 2 - 1;
        Respawn(xPolarity,  yPolarity);

    }

    private void Respawn(int xdirection, int ydirection)
    {
        float xspeed = initialVelocity.x * xdirection;
        float yspeed = initialVelocity.y * ydirection;
        rb.linearVelocity = new Vector2(xspeed, yspeed);

        float xLocation = Random.Range(minX, maxX);
        float yLocation = Random.Range(minY, maxY);
        transform.position = new Vector2(xLocation, yLocation);
        FindAnyObjectByType<PlayerControllerTask7>().AddPoints(1);
    }
}
