using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    [Header("Settings")]
    public float backgroundVolume = 0.5f;
    public float effectVolume = 1f;

    [Header("Sources")]
    public AudioSource backgroundAudio;
    public AudioSource effectAudio;

    [Header("Files")]
    public AudioClip clickSound;
    public AudioClip deathSound;
    public AudioClip shipUpgradeSound;
    public AudioClip fortUpgradeSound;
    public List<AudioClip> cannonSound;
    public List<AudioClip> hitSound;


    public enum SoundType
    {
        Click,
        Death,
        ShipUpgrade,
        FortUpgrade,
        Cannon,
        Hit
    }


    #region singleton
    // singleton
    public static AudioManager instance;
    private void Awake()
    {
        if (instance && instance != this) { Destroy(gameObject); return; }
        instance = this;
    }
    #endregion



    public void PlayThemeMusic()
    {
        backgroundAudio.volume = 0;
        backgroundAudio.Play();
        backgroundAudio.DOFade(backgroundVolume, 2f);
    }

    public void StopThemeMusic()
    {
        backgroundAudio.DOFade(0, 2f).OnComplete(() =>
        {
            backgroundAudio.Stop();
        });
    }


    public void PlaySoundEffect(SoundType soundType)
    {
        switch (soundType)
        {
            case SoundType.Click:
                effectAudio.PlayOneShot(clickSound);
                break;
            case SoundType.Death:
                effectAudio.PlayOneShot(deathSound);
                break;
            case SoundType.ShipUpgrade:
                effectAudio.PlayOneShot(shipUpgradeSound);
                break;
            case SoundType.FortUpgrade:
                effectAudio.PlayOneShot(fortUpgradeSound);
                break;
            case SoundType.Cannon:
                effectAudio.PlayOneShot(cannonSound[Random.Range(0, cannonSound.Count)]);
                break;
            case SoundType.Hit:
                effectAudio.PlayOneShot(hitSound[Random.Range(0, hitSound.Count)]);
                break;
            default:
                break;
        }
    }



	
}
