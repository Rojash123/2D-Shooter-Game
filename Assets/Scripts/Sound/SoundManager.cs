using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] SoundSO soundSO;
    public AudioSource bgAudioSource, sfxAudioSource, spinWheelSfxSource;
    private bool isSfxMuted, isBgMuted;

    [SerializeField] GameObject sfxOn;
    [SerializeField] GameObject bgOn;

    private const string sfxPrefs = "sfx";
    private const string bgPrefs = "bg";

    public bool IsSfxMuted
    {
        get { return isSfxMuted; }
        set
        {
            isSfxMuted = value;
            sfxOn.SetActive(!value);
            sfxAudioSource.mute = value;
            spinWheelSfxSource.mute = value;
            UIbuttonClick();
            PlayerPrefs.SetInt(sfxPrefs,value==true?0:1);
        }
    }
    public bool IsBgMuted
    {
        get { return isBgMuted; }
        set
        {
            isBgMuted = value;
            bgOn.SetActive(!value);
            bgAudioSource.mute = value;
            UIbuttonClick();
            PlayerPrefs.SetInt(bgPrefs, value==true ? 0 : 1);
        }
    }
    private void PlaySfxSound(SFXSoundType type)
    {
        if (isSfxMuted) return;
        sfxAudioSource.PlayOneShot(soundSO.GetSound(type));
    }
    private void Start()
    {
        if (!PlayerPrefs.HasKey(sfxPrefs))
        {
            IsBgMuted = false;
            IsSfxMuted = false;
        }
        else
        {
            IsSfxMuted= PlayerPrefs.GetInt(sfxPrefs)==0?true:false;
            IsBgMuted = PlayerPrefs.GetInt(bgPrefs) == 0 ?true:false;
        }
        PlayBackGroundMusicMenu();
    }

    #region SFX Functions
    public void UIbuttonClick() => PlaySfxSound(SFXSoundType.uiButtonClick);
    public void UIPanelSlide() => PlaySfxSound(SFXSoundType.uiPanelSlider);
    public void GameStart() => PlaySfxSound(SFXSoundType.gameStart);
    public void GameOver() => PlaySfxSound(SFXSoundType.gameOver);
    public void CollectReward() => PlaySfxSound(SFXSoundType.collectReward);
    public void HitSound() => PlaySfxSound(SFXSoundType.hitSound);
    public void PlayerExplosion() => PlaySfxSound(SFXSoundType.playerExplosion);
    public void EnemyFire() => PlaySfxSound(SFXSoundType.enemyFire);
    public void PlayerFire() => PlaySfxSound(SFXSoundType.playerFire);
    public void PowerUpPicked() => PlaySfxSound(SFXSoundType.powerUpPicked);
    public void Invincibility() => PlaySfxSound(SFXSoundType.invincibility);
    public void FireRate() => PlaySfxSound(SFXSoundType.fireRate);
    public void MultiShot() => PlaySfxSound(SFXSoundType.multiShot);
    public void UpgradeFailed() => PlaySfxSound(SFXSoundType.UpgradeFailed);
    public void Upgraded() => PlaySfxSound(SFXSoundType.UpgradeSuccess);
    public void CoinCollect() => PlaySfxSound(SFXSoundType.coinCollect);

    #endregion

    #region BgMusic
    public void PlayBackGroundMusicMenu()
    {
        if (isBgMuted) return;
        bgAudioSource.clip = soundSO.GetSound(SFXSoundType.bgMusicLobby);
        bgAudioSource.Play();
        bgAudioSource.volume = 1;
    }
    public void PlayBackGroundMusicGame()
    {
        if (isBgMuted) return;
        bgAudioSource.clip = soundSO.GetSound(SFXSoundType.bgMusicGame);
        bgAudioSource.volume = 0.1f;
        bgAudioSource.Play();
    }
    #endregion

    #region SpinWheelMusic
    public void PlaySpinWheel()
    {
        if (isSfxMuted) return;
        bgAudioSource.Play();
    }
    public void StopSpinWheel()
    {
        bgAudioSource.Stop();
    }
    #endregion
}
