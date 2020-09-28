using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private static PlayerData instance;

    public int Score { get; set; }
    public int Lives { get; set; }
    public int CurrentGameLevel { get; set; }
    public int GameLevelPlus { get; set; }
    public int TotalGameLevel { get; set; }

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;

            // Setup default player's data
            ResetPlayerData();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetPlayerData()
    {
        Lives = 5;
        Score = 0;
        CurrentGameLevel = 1;
        GameLevelPlus = 0;
        TotalGameLevel = 1;
    }
}
