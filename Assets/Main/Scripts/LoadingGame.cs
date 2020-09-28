using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingGame : MonoBehaviour
{
    PlayerData playerData;

    [SerializeField] TextMeshProUGUI livesText = null;
    [SerializeField] TextMeshProUGUI scoreText = null;
    [SerializeField] TextMeshProUGUI gameOverText = null;
    [SerializeField] TextMeshProUGUI currentGameLevelText = null;

    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.Find("PlayerData").GetComponent<PlayerData>();

        livesText.text = "" + playerData.Lives;
        scoreText.text = "" + playerData.Score;
        currentGameLevelText.text = $"Stage {playerData.CurrentGameLevel + playerData.GameLevelPlus * playerData.TotalGameLevel}";

        if (playerData.Lives == 0)
        {
            currentGameLevelText.enabled = false;
            gameOverText.enabled = true;

            StartCoroutine(StartGameOver());
        }
        else
        {
            currentGameLevelText.enabled = true;
            gameOverText.enabled = false;

            StartCoroutine(StartGameLevel());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator StartGameLevel()
    {
        yield return new WaitForSeconds(2.0f);

        SceneManager.LoadScene($"SceneLevel{playerData.CurrentGameLevel}");
    }

    IEnumerator StartGameOver()
    {
        yield return new WaitForSeconds(2.0f);

        playerData.ResetPlayerData();

        SceneManager.LoadScene("MainMenuScene");
    }
}
