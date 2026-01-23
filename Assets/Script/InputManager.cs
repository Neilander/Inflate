using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct MyInput
{
    public static int x;
    public static bool jump;
    public static bool paused;
}

public class InputManager : MonoBehaviour
{
    private KeyCode left;
    private KeyCode right;
    private KeyCode jump;
    private KeyCode quit;
    private KeyCode restart;
    private string sceneName;

    void Start()
    {
        left = SaveAndLoad.gameData.left;
        right = SaveAndLoad.gameData.right;
        jump = SaveAndLoad.gameData.jump;
        quit = SaveAndLoad.gameData.quit;
        restart = SaveAndLoad.gameData.restart;
        sceneName = SceneManager.GetActiveScene().name;
        MyInput.paused = false;
    }

    void Update()
    {
        if (!MyInput.paused)
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
                MyInput.paused = true;
            if (Input.GetKeyDown(restart))
                SceneManager.LoadScene(sceneName);
        }
        if (Input.GetKeyDown(KeyCode.Q))
            MyInput.paused = false;

    }
}
