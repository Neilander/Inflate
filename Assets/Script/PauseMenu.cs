using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private Color32 notSelected = new Color32(255, 255, 255, 255);
    private Color32 Selected = new Color32(255, 255, 100, 255);
    public Camera myCamera;
    public Transform continueGame;
    public SpriteRenderer continueGameSpriteRenderer;
    public Transform restartGame;
    public SpriteRenderer restartGameSpriteRenderer;
    public Transform quit;
    public SpriteRenderer quitSpriteRenderer;

    private Vector2 mousePosition;

    void Update()
    {
        if (GameManager.paused)
        {
            transform.position = myCamera.transform.position + new Vector3(0, 0, 4f);
            mousePosition = myCamera.ScreenToWorldPoint(Input.mousePosition);

            if (Touch(mousePosition, continueGame))
            {
                continueGame.localScale = new Vector3(1, 1, 1);
                continueGameSpriteRenderer.color = Selected;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                    ContinueGame();
            }
            else
            {
                continueGame.localScale = new Vector3(0.8f, 0.8f, 1);
                continueGameSpriteRenderer.color = notSelected;
            }

            if (Touch(mousePosition, restartGame))
            {
                restartGame.localScale = new Vector3(1, 1, 1);
                restartGameSpriteRenderer.color = Selected;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                    RestartGame();
            }
            else
            {
                restartGame.localScale = new Vector3(0.8f, 0.8f, 1);
                restartGameSpriteRenderer.color = notSelected;
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
        else
        {
            transform.position = myCamera.transform.position + new Vector3(0, 20f, 0);
        }
    }

    private bool Touch(Vector2 position, Transform button)
    {
        return (position.x >= button.position.x - 2 * button.lossyScale.x && position.x <= button.position.x + 2 * button.lossyScale.x && position.y <= button.position.y + 0.5f * button.lossyScale.y && position.y >= button.position.y - 0.5f * button.lossyScale.y);
    }

    private void ContinueGame()
    {
        GameManager.paused = false;
        Time.timeScale = 1;
    }

    private void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Quit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene");
    }
}
