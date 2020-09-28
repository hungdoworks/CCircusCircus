using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxController : MonoBehaviour
{
    [SerializeField] List<AudioClip> sfxList;

    AudioSource audioPlayer;

    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayJumpSfx()
    {
        audioPlayer.PlayOneShot(sfxList[0]);
    }

    public void PlayCrashedSfx()
    {
        audioPlayer.PlayOneShot(sfxList[1]);
    }

    public void PlayLevelFailed()
    {
        audioPlayer.PlayOneShot(sfxList[2]);
    }

    public void PlayPauseGame()
    {
        audioPlayer.PlayOneShot(sfxList[3]);
    }

    public void PlayLevelComplete()
    {
        audioPlayer.PlayOneShot(sfxList[4]);
    }

    public void PlayTrampolineSfx()
    {
        audioPlayer.PlayOneShot(sfxList[5]);
    }

    public void PlayCalculateScoreSfx()
    {
        audioPlayer.PlayOneShot(sfxList[6]);
    }

    public float GetLevelFailedSfxLength()
    {
        return sfxList[2].length;
    }

    public float GetLevelCompleteSfxLength()
    {
        return sfxList[4].length;
    }
}
