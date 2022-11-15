using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOver;
    [SerializeField] TMP_Text gameOverText;
    [SerializeField] Lubang lubang;
    [SerializeField] PlayerController player;


    private void Start()
    {
        gameOver.SetActive(false);
    }

    private void Update()
    {
        if(lubang.Entered && gameOver.activeInHierarchy == false)
        {
            gameOver.SetActive(true);
            gameOverText.text = "You Won!  Your Score : 0" + player.ShootCount;
        }
    }

    public void BackToMainMenu()
    {
        SceneLoader.Load("MainMenu");
    }

    public void Replay()
    {
        SceneLoader.ReloadLevel();
    }

    public void PlayNext()
    {
        SceneLoader.LoadNextLevel();
    }
}
