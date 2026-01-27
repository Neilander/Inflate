using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Transform hero;
    public Transform flag;
    public float winAreaWidth;
    public float winAreaHeight;
    public static bool paused;
    public int level;

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
        else if (hero.position.x - flag.position.x < winAreaWidth / 2f && hero.position.x - flag.position.x > - winAreaWidth / 2f && hero.position.y - flag.position.y < 0.5f + winAreaWidth / 2f && hero.position.y - flag.position.y > 0.5f - winAreaWidth / 2f)
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
        if (LevelManager.sceneNames.Count >= level + 2)
            SceneManager.LoadScene(LevelManager.sceneNames[level + 1]);
        else
            SceneManager.LoadScene("GameOverScene");
    }
}
