using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScene : MonoBehaviour
{
    public float cooldown = 1f;
    float timer;

    void Start()
    {
        timer = cooldown;
    }

    void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            return;
        }

        if (Input.anyKeyDown)
            SceneManager.LoadScene("MenuScene");
    }
}