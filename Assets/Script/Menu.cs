using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private Color32 notSelected = new Color32(255, 255, 255, 255);
    private Color32 Selected = new Color32(255, 255, 100, 255);
    public List<string> sceneNames;
    public Camera myCamera;
    public Transform startGame;
    public SpriteRenderer startGameSpriteRenderer;
    public Transform selectLevel;
    public SpriteRenderer selectLevelSpriteRenderer;
    public Transform quit;
    public SpriteRenderer quitSpriteRenderer;

    private Vector2 mousePosition;

    void Start()
    {
        SaveAndLoad.Load(0);
    }

    void Update()
    {
        mousePosition = myCamera.ScreenToWorldPoint(Input.mousePosition);
        if (Touch(mousePosition, startGame))
        {
            startGame.localScale = new Vector3(1, 1, 1);
            startGameSpriteRenderer.color = Selected;
            if (Input.GetKeyDown(KeyCode.Mouse0))
                StartGame();
        }
        else
        {
            startGame.localScale = new Vector3(0.8f, 0.8f, 1);
            startGameSpriteRenderer.color = notSelected;
        }
            
        if (Touch(mousePosition, selectLevel))
        {
            selectLevel.localScale = new Vector3(1, 1, 1);
            selectLevelSpriteRenderer.color = Selected;
            if (Input.GetKeyDown(KeyCode.Mouse0))
                SelectLevel();
        }
        else
        {
            selectLevel.localScale = new Vector3(0.8f, 0.8f, 1);
            selectLevelSpriteRenderer.color = notSelected;
        }
           
        if (Touch(mousePosition, quit))
        {
            quit.localScale = new Vector3(1, 1, 1);
            quitSpriteRenderer.color = Selected;
            if (Input.GetKeyDown(KeyCode.Mouse0))
                Quit();
        }
        else
        {
            quit.localScale = new Vector3(0.8f, 0.8f, 1);
            quitSpriteRenderer.color = notSelected;
        }
    }

    private bool Touch(Vector2 position, Transform button)
    {
        return (position.x >= button.position.x - 2 * button.lossyScale.x && position.x <= button.position.x + 2 * button.lossyScale.x && position.y <= button.position.y + 0.5f * button.lossyScale.y && position.y >= button.position.y - 0.5f * button.lossyScale.y);
    }

    private void StartGame()
    {
        if(sceneNames.Count >= SaveAndLoad.gameData.nowLevel - 1)
            SceneManager.LoadScene(sceneNames[SaveAndLoad.gameData.nowLevel]);
        else
            SceneManager.LoadScene(sceneNames[0]);
    }

    private void SelectLevel()
    {
        SceneManager.LoadScene("LevelScene");
    }

    private void Quit()
    {
        SaveAndLoad.Save(0);
        Application.Quit();
    }
}
