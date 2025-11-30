using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public List<GameObject> panel;
    [SerializeField] GameObject player;
    private bool isWheelSpinning;
    private int doubleValue;

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
    [SerializeField] GameObject doubleRewardGameOver, gameOver;

    [Space(10)]
    [Header("Reward CollectPanel")]
    [SerializeField] GameObject rewardCollectPanel;
    [SerializeField] TextMeshProUGUI rewardAmount,coinGameOver;
    [SerializeField] Button rewardCollectButton;
    [SerializeField] GameObject loadingAd;

    [Space(10)]
    [Header("Buttons")]
    [SerializeField] Button startGameBtn;
    [SerializeField] Button pauseGameBtn;
    [SerializeField] Button unPauseGameBtn;
    [SerializeField] Button RetryBtn;
    [SerializeField] Button menuBtn;
    [SerializeField] Button quitGameBtn;
    [SerializeField] Button watchAdButton;
    [SerializeField] Button laterBtn;


    [Space(10)]
    [Header("Stats")]
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI timeSpent;
    public TextMeshProUGUI userName;

    [Space(10)]
    [Header("PowerUp UpgradeUI")]
    public List<Upgrade> upgradeItemsLits;
    public TextMeshProUGUI walletCoinValue;

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
        RetryBtn.onClick.AddListener(StartGame);
        pauseGameBtn.onClick.AddListener(PauseGame);
        unPauseGameBtn.onClick.AddListener(UnpauseGame);
        menuBtn.onClick.AddListener(GoToHome);
        quitGameBtn.onClick.AddListener(HandeQuitMidGame);
        watchAdButton.onClick.AddListener(WatchAd);
        rewardCollectButton.onClick.AddListener(() => 
        {
            rewardCollectButton.interactable = false;
            rewardCollectPanel.SetActive(false);
            SoundManager.Instance.UIbuttonClick();
        });
        laterBtn.onClick.AddListener(LaterButton);
    }
    void RemoveButtonListeners()
    {
        startGameBtn.onClick.RemoveAllListeners();
        RetryBtn.onClick.RemoveAllListeners();
        pauseGameBtn.onClick.RemoveAllListeners();
        unPauseGameBtn.onClick.RemoveAllListeners();
        menuBtn.onClick.RemoveAllListeners();
        quitGameBtn.onClick.RemoveAllListeners();
        rewardCollectButton.onClick.RemoveAllListeners();
        watchAdButton.onClick.RemoveAllListeners();
        laterBtn.onClick.RemoveAllListeners();
    }
    public void FooterButton(int value)
    {
        if (isWheelSpinning) return;
        foreach (GameObject go in panel)
        {
            go.SetActive(false);
        }
        panel[value].SetActive(true);
        SoundManager.Instance.UIPanelSlide();
    }

    #region Listen Events
    void HandlePanelOnGameStart()
    {
        LobbyUI.SetActive(false);
        inGameUI.SetActive(true);
        gameOverUI.SetActive(false);
    }
    void HandlePanelOnGameEnd(bool isQuit)
    {
        if (isQuit) return;
        Invoke(nameof(GameOverUI), 1f);
    }
    void GameOverUI()
    {
        gamePausedUI.SetActive(false);
        LobbyUI.SetActive(false);
        gameOverUI.SetActive(true);
        doubleRewardGameOver.SetActive(true);
        gameOver.SetActive(false);
    }
    void HandleGamePause(bool isPaused)
    {
        Time.timeScale = isPaused ? 0 : 1;
        gamePausedUI.SetActive(isPaused);
    }
    void HandleSpinButtonPress()
    {
        loadingAd.SetActive(true);
        AdsManager.Instance.ShowRewardedAd(HandleSpinWheelCallBack);
    }
    void HandleSpinWheelCallBack(bool isSuccess)
    {
        loadingAd.SetActive(false);
        if (isSuccess)
        {
            isWheelSpinning= true;
            gameEventsSO.OnstartSpinWheel?.Invoke();
        }
        else
        {

        }
    }
    void HandleWatchAd(int coin)
    {
        loadingAd.SetActive(true);
        AdsManager.Instance.ShowRewardedAd(RewardCallbackFreeCoin);
    }
    void RewardCallbackFreeCoin(bool isSuccess)
    {
        loadingAd.SetActive(false);
        if (isSuccess)
        {
            HandleRewardCollect(200);
        }
    }

    void HandleDoubleReward(int coin)
    {
        doubleValue = coin;
        loadingAd.SetActive(true);
        LaterButton();
        AdsManager.Instance.ShowRewardedAd(RewardCallbackDouble);
    }
    void RewardCallbackDouble(bool isSuccess)
    {
        loadingAd.SetActive(false);
        if (isSuccess)
        {
            HandleRewardCollect(doubleValue);
            rewardAmount.text = (doubleValue * 2).ToString();
            coinGameOver.text = (doubleValue * 2).ToString();
        }
    }
    void HandleRewardCollect(int coin)
    {
        isWheelSpinning = false;
        SaveDataManager.Instance.HandleScoresAndCoin(coin,0,0);
        rewardAmount.text = coin.ToString();
        rewardCollectPanel.SetActive(true);
        SoundManager.Instance.CollectReward();
    }
    void HandleShopData(SaveData data)
    {
        walletCoinValue.text=data.coin.ToString();
        foreach (var item in data.powerupdata)
        {
            var upgradeItem = upgradeItemsLits.FirstOrDefault(x => x.powerUpType == item.powerUpType);
            upgradeItem.Duration = item.duration;
            upgradeItem.Level = item.level;
        }
        var bulletItem = upgradeItemsLits.FirstOrDefault(x => x.powerUpType == PowerUps.None);
        bulletItem.Level = data.bulletlevel;
        highScoreText.text=data.highScore.ToString("f2");
        timeSpent.text =  (Mathf.Floor(data.timeSpent/60))+" min";
        userName.text = data.userName.ToString();
    }
    #endregion
    #region AssignFunctionToUI
    private void StartGame()
    {
        player.SetActive(true);
        gameEventsSO.OnGameStart?.Invoke();
        SoundManager.Instance.UIbuttonClick();
        SoundManager.Instance.PlayBackGroundMusicGame();
    }
    private void PauseGame()
    {
        uIEventsSO.OnGamePaused?.Invoke(true);
        SoundManager.Instance.UIbuttonClick();
        SoundManager.Instance.PlayBackGroundMusicMenu();
    }
    private void UnpauseGame()
    {
        uIEventsSO.OnGamePaused?.Invoke(false);
        SoundManager.Instance.UIbuttonClick();
        SoundManager.Instance.PlayBackGroundMusicGame();
    }
    private void WatchAd()
    {
        SoundManager.Instance.UIbuttonClick();
        uIEventsSO.OnWatchAdPressed?.Invoke(200);
    }
    private void GoToHome()
    {
        LobbyUI.SetActive(true);
        inGameUI.SetActive(false);
        gameOverUI.SetActive(false);
        player.SetActive(true);
        SoundManager.Instance.UIbuttonClick();
        SoundManager.Instance.PlayBackGroundMusicMenu();
    }
    private void HandeQuitMidGame()
    {
        uIEventsSO.OnGamePaused?.Invoke(false);
        gameEventsSO.OnGameOver?.Invoke(true);
        gamePausedUI.SetActive(false);
        gameOverUI.SetActive(false);
        inGameUI.SetActive(false);
        LobbyUI.SetActive(true);
        SoundManager.Instance.PlayBackGroundMusicMenu();
    }
    private void LaterButton()
    {
        doubleRewardGameOver.SetActive(false);
        gameOver.SetActive(true);
        SoundManager.Instance.GameOver();
    }

    #endregion

}
