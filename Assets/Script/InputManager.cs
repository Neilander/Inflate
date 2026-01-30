using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct MyInput
{
    public static int x;
    public static bool jump;
}

public class InputManager : MonoBehaviour
{
    public GameManager gameManager;
    private KeyCode left;
    private KeyCode right;
    private KeyCode jump;
    private KeyCode slow;
    private KeyCode quit;
    private KeyCode restart;

    void Start()
    {
        left = SaveAndLoad.gameData.left;
        right = SaveAndLoad.gameData.right;
        jump = SaveAndLoad.gameData.jump;
        slow = SaveAndLoad.gameData.slow;
        quit = SaveAndLoad.gameData.quit;
        restart = SaveAndLoad.gameData.restart;
    }

    void Update()
    {
        if (!GameManager.paused)
        {
            if (Input.GetKeyDown(left))
                MyInput.x = -1;
            if (Input.GetKeyDown(right))
                MyInput.x = 1;
            if (Input.GetKeyUp(left))
            {
                if (Input.GetKey(right))
                    MyInput.x = 1;
                else
                    MyInput.x = 0;
            }
            if (Input.GetKeyUp(right))
            {
                if (Input.GetKey(left))
                    MyInput.x = -1;
                else
                    MyInput.x = 0;
            }
            MyInput.jump = Input.GetKeyDown(jump);
            if (Input.GetKeyDown(quit))
                GameManager.paused = true;
            GameManager.slow = Input.GetKey(slow);
            if (Input.GetKeyDown(restart))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (Input.GetKeyDown(quit))
        {
            GameManager.paused = false;
            Time.timeScale = 1;
        }
    }
}
