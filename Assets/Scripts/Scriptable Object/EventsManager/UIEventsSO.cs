using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UIEventsSO", menuName = "Scriptable Objects/UIEventsSO")]
public class UIEventsSO : ScriptableObject
{
    public Action OnSpinButtonPressed;

    public Action<int> OnDoubleReward;
    public Action<bool> OnGamePaused;
    public Action<int> OnWatchAdPressed;
    public Action<int> OnrewardCollect;

    public Action<int> OnBeginPurchasePowerUp;
    public Action<PowerUpsClass> OnUpgradePowerUpCompleted;
}
