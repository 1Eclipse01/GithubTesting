using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerTask7 : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private int score = 0;
    [SerializeField] private int lives = 3;
    [SerializeField] float speed = 15;
    [SerializeField] float ProjectileSpeed = 500;
    private Vector2 projectileDirection = new Vector2(1, 0);
    private Vector2 moveInput;

    [Header("Components")]
    public Rigidbody2D rb;
    [SerializeField] private AudioSource speaker;

    [Header("External")]
    [SerializeField] TextMeshProUGUI ScoreTextBox;
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] GameObject reticle;
    [SerializeField] AudioClip scoreSound;
    [SerializeField] AudioClip shootSound;
    [SerializeField] GameObject explosionEffect;
    [SerializeField] GameObject projectilePrefab;


    void UpdateTextBoxes()
    {
        ScoreTextBox.text = score.ToString();
        }
    private void OnGUI()
    {
        int startX = 1750;
        int spacing = 60;
        Texture livesTexture = GetComponent<SpriteRenderer>().sprite.texture;
        for (int livesDrawn = 0; livesDrawn < lives; livesDrawn = livesDrawn + 1)
        {
            int xPosition = startX - spacing * livesDrawn;
            int yPosition = 50;
            int xSize = 50;
            int ySize = 50;
            Rect drawLocation = new Rect(x: xPosition, y: yPosition, width: xSize, height: ySize);
            GUI.DrawTexture(drawLocation, livesTexture); 
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speaker = GetComponent<AudioSource>();

        string newname = "";
        foreach (char letter in name)
        {
            if (char.IsUpper(letter)) newname += " ";
            newname += char.ToUpper(letter);
        }
        name = newname;
    }

    void Update()
    {
        if (rb == null) return;
        rb.linearVelocity = new Vector2(moveInput.x * speed, moveInput.y * speed);
        HandleFiring();
        CheckWinAndLoseStates();
        UpdateTextBoxes();
        
    }

    void CheckWinAndLoseStates()
    {
        if (score >= 10)
        {
            GameObject enemyObject = GameObject.Find("Enemy");
            Destroy(enemyObject);
            speaker.PlayOneShot(winSound);
        }
        if (lives <= 0)
        {
            speaker.PlayOneShot(deathSound);
            Destroy(rb);
            GetComponent<SpriteRenderer>().color = Color.red;
            Invoke(nameof(RemoveCorpse), 1.5f);
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void RemoveCorpse()
    {
        Destroy(gameObject);
    }

    void HandleFiring()
    {
        if (rb.linearVelocity !=  Vector2.zero)
        {
            projectileDirection = rb.linearVelocity.normalized;
        }
        reticle.transform.localPosition = projectileDirection;
    }

    public void OnFire(InputValue value)
    {
        Vector2 spawnnLocation = transform.position;
        Quaternion spawnRotation = projectilePrefab.transform.rotation;
        GameObject spawnedProjectile = Instantiate(projectilePrefab, spawnnLocation, spawnRotation);
        spawnedProjectile.GetComponent<Rigidbody2D>().AddForce(projectileDirection * ProjectileSpeed);
        speaker.PlayOneShot(shootSound);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyControllerTask7>() != null)
        {
            lives--;
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
    }

    public void AddPoints(int points)
    {
        score += points;
        speaker.PlayOneShot(scoreSound);
    }
    
}
