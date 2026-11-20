using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerTask8B : MonoBehaviour
{
    private AudioSource[] soundPlayers;
    [SerializeField] private float bulletSpeed = 400;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject targetPrefab;
    private bool gameInPlay;
    [SerializeField] private TextMeshProUGUI textDisplay;
    private int bullets = 0;
    private int score = 0;
    private int round = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(gameInPlay)
        {
            HandleAiming();
            HandleShooting();
        }
        else
        {
            if(Keyboard.current.pKey.wasReleasedThisFrame)
            {
                // TODO: play the sound here!
                Invoke(nameof(StartNewGame),1);
            }
        }
    }

    private void StartNewGame()
    {
        round = 0;
        gameInPlay = true;
        score = 0;
        StartNewRound();
    }

    private void StartNewRound()
    {
        round++;        // we track which round we are on.  so that number must go up!
        bullets = 6;    // each round starts with a fresh clip of ammo

        // TODO: Spawn 5 targets and put them in the list
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        textDisplay.text = $"Bullets left: {bullets}.   Shots hit: {score}";
    }

    private void HandleShooting()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && bullets > 0)
        {
            soundPlayers[0].Play();
            bullets--;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
            bullet.GetComponent<Rigidbody>().linearVelocity = transform.forward * bulletSpeed;
            Destroy(bullet, 0.5f);
            if (bullets <= 0)
            {
                Invoke(nameof(CheckForLoseState), 1.5f);
            }
            UpdateDisplay();
        }
    }

    // This function should be called after your last bullet has had time to hit or miss a target
    private void CheckForLoseState()
    {
        // TODO: if no targets left, call GameOver, with appropriate message
    }

    // This function should be called when the game is won or lost - you should provide a win or lose message
    private void GameOver(string message)
    {
        // TODO: destroy all targets and remove them all from the list
        gameInPlay = false;
        textDisplay.text = message + "\nPress P to play again";
    }

    private void HandleAiming()
    {
        Vector2 mouseInput = Mouse.current.delta.value * Time.deltaTime;
        transform.Rotate(Vector3.up, mouseInput.x, Space.World);
        transform.Rotate(transform.right, -mouseInput.y, Space.World);
    }

    public void TargetShot(GameObject StruckTarget)
    {
        score += 1;
        UpdateDisplay();                // Show the player the new score
        soundPlayers[1].Play();         // play the second sound effect

        // TODO:
        // * Remove the target from the list
        // * Destroy the target
        // * If there are 0 targets left, then:
        //    - Change the display text to let player know they have completed the round
        //    - call the 'StartNewRound' function after a 3 second delay
    }
}
