using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmController : MonoBehaviour
{
    AudioSource audioPlayer;

    bool isFirstUpdate;

    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();

        isFirstUpdate = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;

            audioPlayer.Play();
        }
    }

    public void PlayPauseBGM()
    {
        if (audioPlayer.isPlaying)
            audioPlayer.Pause();
        else
            audioPlayer.UnPause();
    }
}
