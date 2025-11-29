using Dan.Main;
using TMPro;
using UnityEngine;

public class LeaderBoardManagaer : Singleton<LeaderBoardManagaer>
{
    [SerializeField] private TMP_Text[] _entryTextObjects;
    [SerializeField] private TMP_InputField _usernameInputField;
    [SerializeField] GameObject setUserNamePanel;

    public void SetUserName()
    {
        SaveDataManager.Instance.SetUserName(_usernameInputField.text);
        PlayerPrefs.SetInt("SetUserName", 0);
        setUserNamePanel.SetActive(false);
        SoundManager.Instance.UIbuttonClick();
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("SetUserName"))
        {
            setUserNamePanel.SetActive(true);
        }
        LoadEntries();
    }

    private void LoadEntries()
    {
        Leaderboards.Scoreleaderboard.GetEntries(entries =>
        {
            foreach (var t in _entryTextObjects)
                t.text = "";

            var length = Mathf.Min(_entryTextObjects.Length, entries.Length);
            for (int i = 0; i < length; i++)
                _entryTextObjects[i].text = $"{entries[i].Rank}. {entries[i].Username} - {entries[i].Score}";
        });
    }

    public void UploadEntry()
    {
        Leaderboards.Scoreleaderboard.UploadNewEntry(SaveDataManager.Instance.currentData.userName, (int)SaveDataManager.Instance.currentData.highScore, isSuccessful =>
        {
            if (isSuccessful)
                LoadEntries();
        });
    }
}
