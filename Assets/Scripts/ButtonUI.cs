using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] private string _level;

    private void Start()
    {

    }

    public void NewGameButton()
    {
        SceneManager.LoadScene(_level);

    }

}
