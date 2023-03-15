using UnityEngine;
using TMPro;


public class WaveDisplay : MonoBehaviour
{
    SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponentInChildren<TMP_Text>().SetText(spawnManager.waveNumber.ToString("Wave: " + 0));
    }
}
