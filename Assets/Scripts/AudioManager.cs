using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioSource backgroundMusicSource;
    public AudioSource soundEffectSource;

    public AudioClip[] backgroundMusicClips;
    public AudioClip[] soundEffectClips;

    private Coroutine currentMusicFadeOutCoroutine;
    void Start()
    {
        // Start playing background music
        PlayBackgroundMusic(backgroundMusicClips[0]);
    }
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Overworld":
                StopBackgroundMusic();
                PlayBackgroundMusic(GameManager.instance.audioManager.backgroundMusicClips[1]);
                break;
            case "PlayerInterior":
                StopBackgroundMusic();
                if (GameManager.instance.customerManager.isOperating && LevelManager.instance.LevelNum == 1)
                    GameManager.instance.audioManager.PlayBackgroundMusic(GameManager.instance.audioManager.backgroundMusicClips[3]);
                else GameManager.instance.audioManager.PlayBackgroundMusic(GameManager.instance.audioManager.backgroundMusicClips[2]);
                break;
        }
    }
    //other class call this method
    public void PlayBackgroundMusic(AudioClip musicClip)
    {
        if (currentMusicFadeOutCoroutine != null)
        {
            StopCoroutine(currentMusicFadeOutCoroutine);
        }

        currentMusicFadeOutCoroutine = StartCoroutine(FadeOutAndChangeMusic(musicClip));
    }

    //other class call this method
    public void PlaySoundEffect(string soundEffectName)
    {
        AudioClip clip = GetSoundEffectClipByName(soundEffectName);
        if (clip != null)
        {
            soundEffectSource.PlayOneShot(clip);
        }
    }

    private AudioClip GetSoundEffectClipByName(string name)
    {
        foreach (AudioClip clip in soundEffectClips)
        {
            if (clip.name == name)
            {
                return clip;
            }
        }
        Debug.LogWarning("Sound effect not found: " + name);
        return null;
    }

    public void StopBackgroundMusic()
    {
        backgroundMusicSource.Stop();
    }

    private IEnumerator FadeOutAndChangeMusic(AudioClip newMusicClip, float fadeDuration = 1.0f)
    {
        float startVolume = backgroundMusicSource.volume;

        // Fade out current music
        while (backgroundMusicSource.volume > 0)
        {
            backgroundMusicSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        backgroundMusicSource.Stop();
        backgroundMusicSource.clip = newMusicClip;
        backgroundMusicSource.Play();

        // Fade in new music
        while (backgroundMusicSource.volume < startVolume)
        {
            backgroundMusicSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        backgroundMusicSource.volume = startVolume;
        currentMusicFadeOutCoroutine = null;
    }
}
