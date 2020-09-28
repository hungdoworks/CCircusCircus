using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    public bool IsGameOver { get; set; }
    public bool IsGamePaused { get; set; }
    public bool IsLevelComplete { get; set; }

    [SerializeField] TextMeshProUGUI countDownText = null;
    [SerializeField] TextMeshProUGUI livesText = null;
    [SerializeField] TextMeshProUGUI scoreText = null;
    [SerializeField] TextMeshProUGUI pauseText = null;

    [SerializeField] int TotalGameLevel = 5;

    PlayerData playerData = null;
    BgmController bgmController;
    SfxController sfxController;
    PlayerController playerController;

    float countDownTime = 300.0f;
    int playerLives = 3;

    int currentGameLevel = 1;

    public int TimeLeft
    {
        get { return Mathf.FloorToInt(countDownTime * 100 / 60); }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerData = GameObject.Find("PlayerData").GetComponent<PlayerData>();
        playerLives = playerData.Lives;
        currentGameLevel = playerData.CurrentGameLevel;
        playerData.TotalGameLevel = TotalGameLevel;

        bgmController = GameObject.Find("BGM").GetComponent<BgmController>();
        sfxController = GameObject.Find("SFX").GetComponent<SfxController>();

        livesText.text = "" + playerLives;
        scoreText.text = "" + playerData.Score;
        IsGameOver = false;
        IsGamePaused = false;
        IsLevelComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLevelComplete == false && IsGameOver == false && countDownTime > 0.0f)
        {
            countDownTime -= Time.deltaTime;

            if (countDownTime < 0.0f)
                countDownTime = 0.0f;

            countDownText.text = $"{TimeLeft}";
        }
        else if (!IsLevelComplete)
        {
            LevelFailed();
        }
    }

    public void LevelComplete()
    {
        IsLevelComplete = true;

        bgmController.PlayPauseBGM();
        sfxController.PlayLevelComplete();

        StartCoroutine("GoToNextLevel");
    }

    public void LevelFailed()
    {
        if (IsGameOver == false)
        {
            playerController.UseUserInput = false;
            IsGameOver = true;
            playerLives--;

            playerData.Lives = playerLives;

            bgmController.PlayPauseBGM();

            StartCoroutine(OnLevelFailed());
        }
    }

    public void PauseResumeGame()
    {
        IsGamePaused = !IsGamePaused;

        Time.timeScale = IsGamePaused ? 0.0f : 1.0f;
        pauseText.gameObject.SetActive(IsGamePaused);

        bgmController.PlayPauseBGM();
        sfxController.PlayPauseGame();
    }

    IEnumerator GoToNextLevel()
    {
        yield return new WaitForSeconds(sfxController.GetLevelCompleteSfxLength());

        int temp = TimeLeft;
        int temp2 = playerData.Score;

        // Calculate final score
        playerData.Score += TimeLeft;

        // Animation for calculating final score
        for (int i = 0; i < temp; i += 10)
        {
            sfxController.PlayCalculateScoreSfx();

            countDownTime -= 10;

            if (countDownTime <= 1)
            {
                countDownTime = 0;
                countDownText.text = $"{TimeLeft}";
                scoreText.text = "" + (temp2 + temp);

                break;
            }
            else
            {
                countDownText.text = $"{TimeLeft}";
                scoreText.text = "" + (temp2 + i);

                yield return new WaitForSeconds(0.05f);
            }
        }

        if (currentGameLevel < TotalGameLevel)
        {
            currentGameLevel++;
        }
        else
        {
            playerData.GameLevelPlus++;
            currentGameLevel = 1;
        }

        playerData.CurrentGameLevel = currentGameLevel;

        yield return new WaitForSeconds(2.0f);

        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator OnLevelFailed()
    {
        yield return new WaitForSeconds(0.8f);

        sfxController.PlayLevelFailed();

        yield return new WaitForSeconds(sfxController.GetLevelFailedSfxLength() - 1);

        playerController.UseUserInput = true;

        SceneManager.LoadScene("LoadingScene");
    }
}
