using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveDataManager : Singleton<SaveDataManager>
{
    private const string fileName = "ImportantFiles";
    private const string directoryName = "Shooter2D";

    public SaveData currentData;
    [SerializeField] GameEventsSO gameEventsSO;

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
        if (!Directory.Exists(Application.persistentDataPath+directoryName))
            Directory.CreateDirectory(Application.persistentDataPath+directoryName);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream saveFile = File.Create($"{Application.persistentDataPath+directoryName}/{fileName}.bin");
        bf.Serialize(saveFile, currentData);
        saveFile.Close();
    }
    public void LoadFile()
    {
        if (!Directory.Exists(Application.persistentDataPath + directoryName))
        {
            CreateNewfile();
            return;
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream saveFile = File.Open($"{Application.persistentDataPath + directoryName}/{fileName}.bin", FileMode.Open);
        currentData = (SaveData)bf.Deserialize(saveFile);
        gameEventsSO.OnDataLoadedAndUpdated?.Invoke(currentData);
    }

    void CreateNewfile()
    {
        List<PowerupData> listData = new List<PowerupData>();
        PowerupData powerup1 = new PowerupData(PowerUps.invincibility, 15, 0);
        PowerupData powerup2 = new PowerupData(PowerUps.increasedFireRate, 15, 0);
        PowerupData powerup3 = new PowerupData(PowerUps.enhancedAttack, 15, 0);
        listData.Add(powerup1);
        listData.Add(powerup2);
        listData.Add(powerup3);
        currentData = new SaveData(0, 100, listData, false,0);
        gameEventsSO.OnDataLoadedAndUpdated?.Invoke(currentData);
        SaveData();
    }
}
