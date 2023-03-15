using UnityEngine;
using TMPro;


public class LivesDisplay : MonoBehaviour
{
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponentInChildren<TMP_Text>().SetText(gameManager.lives.ToString("Lives: " + 0));
    }
}
