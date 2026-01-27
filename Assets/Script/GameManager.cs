using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Transform hero;
    public Transform flag;
    public static bool paused;
    public int level;
    public string sceneName;
    public string nextSceneName;

    void Start()
    {
        Application.targetFrameRate = 60;
        paused = false;
        MyInput.x = 0;
    }

    void FixedUpdate()
    {
        if(hero == null || flag == null)
        {
            Warn();
        }
        else if (hero.position.x - flag.position.x < 0.5f && hero.position.x - flag.position.x > -0.5f && hero.position.y - flag.position.y < 0.55f && hero.position.y - flag.position.y > 0.45f)
        {
            Win();
        }
    }

    private void Warn()
    {

    }

    private void Win()
    {
        SaveAndLoad.gameData.nowLevel = level + 1;
        SaveAndLoad.gameData.level = Mathf.Max(SaveAndLoad.gameData.level, level);
        SaveAndLoad.Save(0);
        SceneManager.LoadScene(nextSceneName);
    }
}
