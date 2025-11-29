using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinWheelManager : MonoBehaviour
{
    [SerializeField] GameObject focusFrame;
    [SerializeField] GameEventsSO gameEventSO;
    [SerializeField] UIEventsSO uiEventSO;

    [SerializeField] Button adButton;

    private readonly Dictionary<int, int> spinWheelReward = new () { { 0, 5000 }, { 1, 100 }, { 2, 500 }, { 3, 100 }, { 4, 100 }, { 5, 300 }, { 6, 100 }, { 7, 1000 } };
    private void Awake()
    {
        gameEventSO.OnstartSpinWheel += StartSpinWheel;
        adButton.onClick.AddListener(() =>
        {
            uiEventSO.OnSpinButtonPressed?.Invoke();
        });
    }
    private void OnDestroy()
    {
        gameEventSO.OnstartSpinWheel -= StartSpinWheel;
    }
    public void StartSpinWheel()
    {
        SpinWheel(PickIndex());
    }
    void SpinWheel(int index)
    {
        adButton.gameObject.SetActive(false);
        focusFrame.SetActive(true);
        float rotationValue = 45 * index + 360 * Random.Range(10, 15);
        LeanTween.rotateAround(focusFrame, transform.forward, -rotationValue, 7f).setEaseOutQuad().setOnComplete(() => 
        {
            uiEventSO.OnrewardCollect?.Invoke(spinWheelReward[index]);
            adButton.gameObject.SetActive(true);
        });
    }
    int PickIndex()
    {
        var dataPool = Random.Range(1, 100);
        Debug.Log(dataPool);
        if (dataPool < 2)
        {
            return 0;
        }
        else if (dataPool < 5)
        {
            return 7;
        }
        else if (dataPool < 10)
        {
            return 2;
        }
        else if (dataPool < 17)
        {
            return 5;
        }
        else if (dataPool < 30)
        {
            return 6;
        }
        else if (dataPool < 50)
        {
            return 1;
        }
        else if (dataPool < 80)
        {
            return 3;
        }
        else
        {
            return 4;
        }
    }
}
