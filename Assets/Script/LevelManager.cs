using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    public static List<string> sceneNames = new List<string>()
    {

    };

    public List<SpriteRenderer> buttonSpriteRenderers;
    public SpriteRenderer quit;
    public Camera myCamera;
    public Sprite locked;
    private Vector3 mousePosition;
    private bool click;
    private Color32 notSelected = new Color32(255, 255, 255, 255);
    private Color32 Selected = new Color32(255, 255, 100, 255);
    private Color32 lockedColor = new Color32(200, 200, 200, 255);


    void Start()
    {
        for (int n = SaveAndLoad.gameData.level + 1; n < buttonSpriteRenderers.Count; n++)
        {
            buttonSpriteRenderers[n].sprite = locked;
            buttonSpriteRenderers[n].color = lockedColor;
            buttonSpriteRenderers[n].transform.localScale = new Vector3(0.9f, 0.9f, 1);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(SaveAndLoad.gameData.quit))
        {
            SceneManager.LoadScene("MenuScene");
            return;
        }
        mousePosition = myCamera.ScreenToWorldPoint(Input.mousePosition);
        click = Input.GetKeyDown(KeyCode.Mouse0);
        if (BigTouch(mousePosition, quit.transform))
        {
            quit.transform.localScale = new Vector3(1, 1, 1);
            quit.color = Selected;
            if (click)
            {
                SceneManager.LoadScene("MenuScene");
                return;
            }
        }
        else
        {
            quit.transform.localScale = new Vector3(0.9f, 0.9f, 1);
            quit.color = notSelected;
        }
        for (int n = 0; n <= SaveAndLoad.gameData.level; n++)
        {
            if (Touch(mousePosition, buttonSpriteRenderers[n].transform))
            {
                buttonSpriteRenderers[n].transform.localScale = new Vector3(1, 1, 1);
                buttonSpriteRenderers[n].color = Selected;
                if (click)
                {
                    SceneManager.LoadScene(sceneNames[n]);
                    return;
                }
            }
            else
            {
                buttonSpriteRenderers[n].transform.localScale = new Vector3(0.9f, 0.9f, 1);
                buttonSpriteRenderers[n].color = notSelected;
            }
        }
    }

    private bool Touch(Vector2 position, Transform button)
    {
        return (position.x >= button.position.x - 0.5f * button.lossyScale.x && position.x <= button.position.x + 0.5f * button.lossyScale.x && position.y <= button.position.y + 0.5f * button.lossyScale.y && position.y >= button.position.y - 0.5f * button.lossyScale.y);
    }

    private bool BigTouch(Vector2 position, Transform button)
    {
        return (position.x >= button.position.x - button.lossyScale.x && position.x <= button.position.x + button.lossyScale.x && position.y <= button.position.y + 0.5f * button.lossyScale.y && position.y >= button.position.y - 0.5f * button.lossyScale.y);
    }
}
