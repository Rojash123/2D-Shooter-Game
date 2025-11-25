using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentValueText, upgradeValueText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] Button purchaseBtn;

    public PowerUps powerUpType;
    private int level;
    private float duration;
    private int cost;

    private int intMaxValue;

    private Dictionary<int, int> costDictionaryPowerUp = new Dictionary<int, int>()
    {
        {0,500},
        {1,1000},
        {2,2000},
        {3,5000},
    };

    private Dictionary<int, int> costDictionaryBulletUpgrade = new Dictionary<int, int>()
    {
        {0,1000},
        {1,2000},
        {2,4000},
    };

    public int Level
    {
        get { return level; }
        set
        {
            level = value;
            intMaxValue = 4;
            if (powerUpType == PowerUps.None)
            {
                intMaxValue = 2;
            }
            if (value < intMaxValue)
            {
                if (powerUpType == PowerUps.None)
                {
                    cost = costDictionaryBulletUpgrade[value];
                    currentValueText.text = $"Firerate {value.ToString()}";
                    upgradeValueText.text = (value + 1).ToString();
                }
                else
                {
                    cost = costDictionaryPowerUp[value];
                }
                costText.text = cost.ToString();
                purchaseBtn.interactable = true;
            }
            else
            {
                purchaseBtn.interactable = false;
                costText.text = "Max";
            }

        }
    }
    public float Duration
    {
        get { return duration; }
        set
        {
            duration = value;
            if (powerUpType != PowerUps.None)
            {
                currentValueText.text = $"Duration {value.ToString()}";
                upgradeValueText.text = (duration + 2.5).ToString("f1");
            }
        }
    }
}
