using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---------------- Audio Clip ----------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------------- Audio Clip ----------------")]
    public AudioClip background;
    public AudioClip bowCharge;
    public AudioClip bowReady;
    public AudioClip bowShoot;
    public AudioClip enemyDeath;
    public AudioClip enemyMadAttack;
    public AudioClip enemyMadCharge;
    public AudioClip enemyMaskBreak;
    public AudioClip playerDamaged;
    public AudioClip playerDeath;

    [Header("---------------- Volume ----------------")]
    public float bgMusicVolume = 0;

    // TODO: no step sfx just yet (player + mad enemy) + no enemy mask break versions just yet

    private void Start()
    {
        musicSource.volume = bgMusicVolume;
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

}
