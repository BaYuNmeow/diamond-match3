using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] destroyNoise;
    public AudioSource backgroundMusic;

    private void Start()
    {
        SetBackgroundMusicVolume();
    }

    private void SetBackgroundMusicVolume()
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            backgroundMusic.volume = PlayerPrefs.GetInt("Sound") == 0 ? 0 : 1;
        }
        else
        {
            backgroundMusic.volume = 1;
        }
        backgroundMusic.Play();
    }

    public void AdjustVolume()
    {
        backgroundMusic.volume = PlayerPrefs.HasKey("Sound") && PlayerPrefs.GetInt("Sound") == 0 ? 0 : 1;
    }

    public void PlayRandomDestroyNoise()
    {
        if (PlayerPrefs.GetInt("Sound", 1) == 1)
        {
            int clipToPlay = Random.Range(0, destroyNoise.Length);
            destroyNoise[clipToPlay].Play();
        }
    }
}
