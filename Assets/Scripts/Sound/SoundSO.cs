using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundSO", menuName = "Scriptable Objects/SoundSO")]
public class SoundSO : ScriptableObject
{

    [Serializable]
    public class Sound
    {
        public SFXSoundType type;
        public AudioClip audioClip;
    }
    public List<Sound> soundList;

    public AudioClip GetSound(SFXSoundType type)
    {
        var item=soundList.FirstOrDefault(s => s.type == type);
        return item.audioClip;
    }
}

public enum SFXSoundType
{
    uiButtonClick,
    uiPanelSlider,
    gameOver,
    gameStart,
    collectReward,
    hitSound,
    playerExplosion,
    enemyFire,
    playerFire,
    powerUpPicked,
    invincibility,
    fireRate,
    multiShot,
    bgMusicLobby,
    bgMusicGame,
    UpgradeSuccess,
    UpgradeFailed,
    coinCollect
}
