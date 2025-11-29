using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class SaveDataManager : Singleton<SaveDataManager>
{
    private const string fileName = "ImportantFiles";
    public SaveData currentData;
    [SerializeField] GameEventsSO gameEventsSO;

    public void HandlePurchase(int cost, PowerUps type,Button purchaseBtn)
    {
        if (cost > currentData.coin)
        {
            Debug.Log("Not Enough Coin");
            SoundManager.Instance.UpgradeFailed();
            purchaseBtn.interactable = true;
            return;
        }
        SoundManager.Instance.Upgraded();
        currentData.coin -= cost;
        if (type == PowerUps.None)
        {
            currentData.bulletlevel += 1;
        }
        else
        {
            var data = currentData.powerupdata.FirstOrDefault(x => x.powerUpType == type);
            data.duration += 2.5f;
            data.level += 1;
        }
        SaveData();
    }
    public void HandleScoresAndCoin(int coinCollected,float score,float time)
    {
        currentData.coin += coinCollected;
        if (score > currentData.highScore)
        {
            currentData.highScore = score;
        }
        currentData.timeSpent += time;
        SaveData();
        LeaderBoardManagaer.Instance.UploadEntry();
    }
    public void SetUserName(string name)
    {
        currentData.userName = name;
        SaveData();
    }
    public override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        LoadFile();
    }
    public void SaveData()
    {
        BinaryFormatter bf = new();
        FileStream saveFile = File.Create($"{Application.persistentDataPath}/{fileName}.bin");
        bf.Serialize(saveFile, currentData);
        saveFile.Close();
        gameEventsSO.OnDataLoadedAndUpdated?.Invoke(currentData);
    }
    public void LoadFile()
    {
        if (!File.Exists($"{Application.persistentDataPath}/{fileName}.bin"))
        {
            CreateNewfile();
            return;
        }
        BinaryFormatter bf = new();
        FileStream saveFile = File.Open($"{Application.persistentDataPath}/{fileName}.bin", FileMode.Open);
        currentData = (SaveData)bf.Deserialize(saveFile);
        saveFile.Close();
        gameEventsSO.OnDataLoadedAndUpdated?.Invoke(currentData);
    }
    void CreateNewfile()
    {
        List<PowerupData> listData = new();
        PowerupData powerup1 = new (PowerUps.invincibility, 15, 0);
        PowerupData powerup2 = new (PowerUps.increasedFireRate, 15, 0);
        PowerupData powerup3 = new (PowerUps.enhancedAttack, 15, 0);
        listData.Add(powerup1);
        listData.Add(powerup2);
        listData.Add(powerup3);
        currentData = new SaveData(0, 1000, listData, false, 0,0,"guest");
        SaveData();
    }
}
