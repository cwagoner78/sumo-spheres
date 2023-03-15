using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool gameOver;
    public  GameOverUI _gameOverUI;
    public int lives = 3;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (lives <= 0)
        {
            gameOver = true;
            Time.timeScale = 0;
            _gameOverUI.gameObject.SetActive(true);
        } 
    }
}
