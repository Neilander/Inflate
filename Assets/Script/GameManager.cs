using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Transform myCamera;
    public Transform hero;
    public Transform flag;
    public Material grid;
    public SpriteRenderer warn;
    public List<Sprite> warnSprite;
    public float winAreaWidth;
    public float winAreaHeight;
    public static bool paused;
    public static bool slow;
    public int level;

    void Start()
    {
        warn.enabled = false;
        Application.targetFrameRate = 60;
        paused = false;
        MyInput.x = 0;
    }

    void FixedUpdate()
    {
        if (paused)
        {
            Time.timeScale = 0;
        }
        else if (slow)
        {
            Time.timeScale = 0.4f;
            grid.color = new Color32(200,200,200,100);
        }
        else
        {
            Time.timeScale = 1;
            grid.color = new Color32(200, 200, 200, 0);
        }
        if (hero == null || flag == null)
        {
            Warn();
        }
        else if (hero.position.x - flag.position.x < winAreaWidth / 2f && hero.position.x - flag.position.x > -winAreaWidth / 2f && hero.position.y - flag.position.y < 0.5f + winAreaWidth / 2f && hero.position.y - flag.position.y > 0.5f - winAreaWidth / 2f)
        {
            Win();
        }
    }

    private void Warn()
    {
        warn.enabled = true;
        warn.transform.position = myCamera.position + new Vector3(0, 0, 3f);
        if (hero == null)
            warn.sprite = warnSprite[0];
        else
            warn.sprite = warnSprite[1];
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
