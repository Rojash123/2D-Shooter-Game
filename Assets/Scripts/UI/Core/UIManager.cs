using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UIManager : MonoBehaviour
{
    public List<GameObject> panel;
    private bool isWheelSpinning;

    [Space(10)]
    [Header("Data Holders")]
    [SerializeField] private UIEventsSO uIEventsSO;
    [SerializeField] private GameEventsSO gameEventsSO;

    [Space(10)]
    [Header("Game Panels")]
    [SerializeField] GameObject LobbyUI;
    [SerializeField] GameObject inGameUI;
    [SerializeField] GameObject gamePausedUI;
    [SerializeField] GameObject gameOverUI;

    [Space(10)]
    [Header("Reward CollectPanel")]
    [SerializeField] GameObject rewardCollectPanel;
    [SerializeField] TextMeshProUGUI rewardAmount;

    [Space(10)]
    [Header("Buttons")]
    [SerializeField] Button startGameBtn;
    [SerializeField] Button pauseGameBtn;
    [SerializeField] Button unPauseGameBtn;

    [Space(10)]
    [Header("PowerUp UpgradeUI")]
    public List<Upgrade> upgradeItemsLits;


    private void Awake()
    {
        uIEventsSO.OnGamePaused += HandleGamePause;
        uIEventsSO.OnSpinButtonPressed += HandleSpinButtonPress;
        uIEventsSO.OnWatchAdPressed += HandleWatchAd;
        uIEventsSO.OnDoubleReward += HandleDoubleReward;
        uIEventsSO.OnrewardCollect += HandleRewardCollect;

        gameEventsSO.OnGameStart += HandlePanelOnGameStart;
        gameEventsSO.OnGameOver += HandlePanelOnGameEnd;
        gameEventsSO.OnDataLoadedAndUpdated += HandleShopData;
    }
    private void OnDestroy()
    {
        uIEventsSO.OnGamePaused -= HandleGamePause;
        uIEventsSO.OnSpinButtonPressed -= HandleSpinButtonPress;
        uIEventsSO.OnWatchAdPressed -= HandleWatchAd;
        uIEventsSO.OnDoubleReward -= HandleDoubleReward;
        uIEventsSO.OnrewardCollect -= HandleRewardCollect;

        gameEventsSO.OnGameStart -= HandlePanelOnGameStart;
        gameEventsSO.OnGameOver -= HandlePanelOnGameEnd;
        gameEventsSO.OnDataLoadedAndUpdated -= HandleShopData;
        RemoveButtonListeners();
    }

    private void Start()
    {
        isWheelSpinning = false;
        AssignButtons();
    }
    void AssignButtons()
    {
        startGameBtn.onClick.AddListener(StartGame);
        pauseGameBtn.onClick.AddListener(PauseGame);
        unPauseGameBtn.onClick.AddListener(UnpauseGame);
    }
    void RemoveButtonListeners()
    {
        startGameBtn.onClick.RemoveAllListeners();
        pauseGameBtn.onClick.RemoveAllListeners();
        unPauseGameBtn.onClick.RemoveAllListeners();
    }
    public void FooterButton(int value)
    {
        if (isWheelSpinning) return;
        foreach (GameObject go in panel)
        {
            go.SetActive(false);
        }
        panel[value].SetActive(true);
    }

    #region Listen Events
    void HandlePanelOnGameStart()
    {
        LobbyUI.SetActive(false);
        inGameUI.SetActive(true);
    }
    void HandlePanelOnGameEnd()
    {
        gamePausedUI.SetActive(false);
        LobbyUI.SetActive(false);
        gameOverUI.SetActive(true);
    }

    void HandleGamePause(bool isPaused)
    {
        Time.timeScale = isPaused ? 0 : 1;
        gamePausedUI.SetActive(isPaused);
    }
    void HandleSpinButtonPress()
    {
        //show ads and then spin the wheel and provide reward
    }
    void HandleSpinWheelCallBack(bool isSuccess, int amount)
    {
        if (isSuccess)
        {

        }
    }
    void HandleWatchAd(int coin)
    {
        //watch ad and provide reward
    }
    void HandleDoubleReward(int coin)
    {
        //watch ad and provide reward
    }
    void RewardCallback(bool isSuccess, int amount)
    {
        if (isSuccess)
        {
            HandleRewardCollect(amount);
        }
    }
    void HandleRewardCollect(int coin)
    {
        PlayerData.UpdateCoinValue(coin);
        rewardAmount.text = coin.ToString();
        rewardCollectPanel.SetActive(true);
    }

    void HandleShopData(SaveData data)
    {
        foreach (var item in data.powerupdata)
        {
            var upgradeItem = upgradeItemsLits.FirstOrDefault(x => x.powerUpType == item.powerUpType);
            upgradeItem.Duration = item.duration;
            upgradeItem.Level = item.level;
        }
        var bulletItem = upgradeItemsLits.FirstOrDefault(x => x.powerUpType == PowerUps.None);
        bulletItem.Level = data.bulletlevel;
    }
    #endregion

    #region AssignFunctionToUI
    private void StartGame()
    {
        gameEventsSO.OnGameStart?.Invoke();
    }
    private void PauseGame()
    {
        uIEventsSO.OnGamePaused?.Invoke(true);
    }
    private void UnpauseGame()
    {
        uIEventsSO.OnGamePaused?.Invoke(false);
    }
    private void WatchAd()
    {
        uIEventsSO.OnWatchAdPressed?.Invoke(200);
    }
    #endregion

}
