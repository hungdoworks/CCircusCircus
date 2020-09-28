using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartGame : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI StartGameText;

    AudioSource audioPlayer;

    bool isGameStarting = false;

    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInputDebug();
    }

    void UpdateInputDebug()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0)
        {
            if (isGameStarting == false)
            {
                isGameStarting = true;

                audioPlayer.Play();

                InvokeRepeating("PlayStartGameTextAnim", 0, 0.3f);

                StartCoroutine(GameStart());
            }
        }
    }

    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(audioPlayer.clip.length);

        SceneManager.LoadScene("LoadingScene");
    }

    void PlayStartGameTextAnim()
    {
        StartGameText.enabled = !StartGameText.enabled;
    }
}
