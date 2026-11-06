using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerTask7 : MonoBehaviour
{
    private int lives = 3;


    private void OnGUI()
    {
        Texture livesTexture = GetComponent<SpriteRenderer>().sprite.texture;
        int repetitions = 0;

        while (repetitions < lives) 
        {
            Rect drawLocation = new Rect(x: 1700 - 40 * repetitions, y: 900, width: 30, height: 30);
            GUI.DrawTexture(drawLocation, livesTexture);
            repetitions = repetitions + 1;
        } 
    }
    void Start()
    {
        int numberOfEnemiesSpawmed = 0;
    }

    void Update()
    {
    }
}
