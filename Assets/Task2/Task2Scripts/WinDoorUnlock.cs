using UnityEngine;

public class WinDoorUnlock : MonoBehaviour
{
    public PlayerControllerTask2 Player;
    [SerializeField] GameObject winDoor;


    // Update is called once per frame
    void Update()
    {
       if(Player.score >= 3f)
        {
            OpenWinDoor();
        }
    }

    public void OpenWinDoor()
    {
        Destroy(winDoor);
    }
}
