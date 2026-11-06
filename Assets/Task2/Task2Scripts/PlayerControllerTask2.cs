using JetBrains.Annotations;
using NUnit.Framework;
using System.Diagnostics.Tracing;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

public class PlayerControllerTask2 : MonoBehaviour
{
    
    public Rigidbody rb;
    

    [Header("Movement")]
    [SerializeField] Vector2 moveInput;
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    


    [Header("GameDetails")]
    public int score = 0;
    [SerializeField] bool hasFinishedLevel = false;
    [SerializeField] float timer = 60.0f;
    [SerializeField] string playerName;
    [SerializeField] Vector3 startPosition;

    [Header("PlayerInfo")]
    [SerializeField] int livesLeft = 3;
    [SerializeField] int damage;
    [SerializeField] bool hasPlayerDied = false;
    

    [Header("TextBoxes")]
    [SerializeField] TextMeshProUGUI ScoreTextBox;
    [SerializeField] TextMeshProUGUI WinScoreTextBox;

    [SerializeField] TextMeshProUGUI TimerTextBox;
    [SerializeField] TextMeshProUGUI WinTimerTextBox;

    [SerializeField] TextMeshProUGUI CompletionTextBox;
    [SerializeField] TextMeshProUGUI WinCompletionTextBox;

    [SerializeField] TextMeshProUGUI LivesTextBox;
    [SerializeField] TextMeshProUGUI CheckpointTextBox;

    [Header("GameStatus")]
    [SerializeField] GameObject LevelFailedImage;
    [SerializeField] GameObject LevelFailedSound;

    [SerializeField] GameObject LevelWonImage;
    [SerializeField] GameObject LevelWonSound;

    [SerializeField] GameObject InteractionBox;

    [SerializeField] GameObject YouDiedImage;
    [SerializeField] GameObject DeathSound;

    [SerializeField] GameObject WinFlag;
    [SerializeField] GameObject star1;
    [SerializeField] GameObject star2;
    [SerializeField] GameObject star3;

    [SerializeField] AudioSource coinCollect;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
   

    void AddPoints(int amount)
    {
        score += amount;

    }

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        ChangeName();
       

    }

    
    // Update is called once per frame
    void Update()
    {
      

        rb.linearVelocity = new Vector3(moveInput.x * speed, rb.linearVelocity.y, moveInput.y * speed);

        if (hasFinishedLevel == false)
        {
            timer = timer - Time.deltaTime;
            UpdateTextBoxes();
        }

        if (timer <= 0.0f)
        {
            hasFinishedLevel = true;
            LevelFailed();
        }
        else if (livesLeft == 0 && hasPlayerDied == false)
            {
            hasFinishedLevel = true;
            LevelFailed();

            }
        else if (livesLeft == 0 && hasPlayerDied == true)
        {
            hasFinishedLevel = true;
        }
        else if (hasFinishedLevel == true)
        {
            rb.linearVelocity = new Vector3(0, 0, 0);
        }

        if(hasPlayerDied == true)
        {
            rb.linearVelocity = new Vector3(0, 0, 0);
        }

        


    }

    void UpdateTextBoxes()
    {
        ScoreTextBox.text = "Your score is " + score.ToString();
        WinScoreTextBox.text = "Your ending score is " + score.ToString();

        TimerTextBox.text = "Timer: " + timer.ToString("F0"); // F0 removes the deimals from the timer.
        WinTimerTextBox.text = "Your time was " + timer.ToString("F0"); 

        CompletionTextBox.text = "Is level completed " + hasFinishedLevel.ToString();
        

        LivesTextBox.text = livesLeft.ToString();
        
    }

    public void LevelFailed() //This triggers once the timer equals 0, meaning the player has failed.
    {
        bool isActive = LevelFailedImage;
        LevelFailedImage.SetActive(isActive);

        rb.linearVelocity = new Vector3(0, 0, 0);

        bool isSoundActive = LevelFailedSound;
        LevelFailedSound.SetActive(isSoundActive); //Sound effect to indicate the level failing.
    }

    void ChangeName()
    {
        playerName = "PlayerBall";
    }

    void LevelCompleted()
    {

        CompletionTextBox.text = "Level Completed";
        bool isWinActive = LevelWonSound;
        LevelWonSound.SetActive(isWinActive);

        rb.linearVelocity = new Vector3(0, 0, 0);

        bool isWinScreen = LevelWonImage;
        LevelWonImage.SetActive(isWinScreen);

        if(timer >= 120 & livesLeft >= 2)
        {
            star1.SetActive(true);
            star2.SetActive(true);
            star3.SetActive(true);
        }
        else if(timer >= 120 & livesLeft == 1)
        {
            star1.SetActive(true);
            star2.SetActive(true);
        }
        else if (timer >= 80 & livesLeft == 2)
        {
            star1.SetActive(true);
            star2.SetActive(true);
        }
        else if (timer >= 40)
        {
            star1.SetActive(true);
        }
    }

   
    public void OnMove(InputAction.CallbackContext cntxt)
    {
        moveInput = cntxt.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext cntxt)
    {
        if(cntxt.started && isGrounded())
        {
            rb.AddForce(new Vector3(0, 100 * jumpForce, 0));
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("KillBarrier"))
        {
            livesLeft = livesLeft - 1;
            DeathSound.SetActive(true);
            bool isDeathActive = YouDiedImage;
            YouDiedImage.SetActive(isDeathActive);
            hasPlayerDied = true;
            Invoke("PlayerDeath", 6.5f);
            Invoke("DeathSoundClose", 7f);
        }

        else if(other.gameObject.CompareTag("Coin"))
        {
            coinCollect.Play();
            score = score + 1;
            Destroy(other.gameObject);
        }
        else if(other.gameObject.CompareTag("Checkpoint"))
        {
            startPosition = other.transform.position;
            bool isCheckpointActive = InteractionBox;
            InteractionBox.SetActive(isCheckpointActive);
            CheckpointTextBox.text = ("Checkpoint Set");
            Destroy(other.gameObject);

            Invoke("InteractionBoxClose", 3f);
        }
        else if(other.gameObject.CompareTag("WinFlag"))
        {
            LevelCompleted();
            hasFinishedLevel = true;
        }
    }
     void InteractionBoxClose()
    {
        
        InteractionBox.SetActive(false);
    }
     
    void DeathSoundClose()
    {
        DeathSound.SetActive(false );  
    }

    bool isGrounded()
    {
      return  Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

    }

    void PlayerDeath()
    {
        transform.position = startPosition;
        YouDiedImage.SetActive(false);
        hasPlayerDied = false;
    }

}
