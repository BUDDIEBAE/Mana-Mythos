using MaskTransitions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TimeManager : Singleton<TimeManager>
{
    [Header("Time Setting")]
    [Tooltip("How many real-time seconds a full day lasts.")]
    [SerializeField] private float dayLengthInSeconds = 600f; // Player will have to sleep so spare 8 hrs in-game for it
    [SerializeField] private float currTime = 0f;
    [SerializeField] private float shopOpenTime = 11f;
    [SerializeField] private float shopCloseTime = 18f;

    [Header("Day Progression")]
    public int currentDay = 1;
    private float timeMultiplier;
    private float timeMultiplierShopClose; // Time When Shop Close run slowly
    private float timeMultiplierShopOpen; // Time When Shop Open run faster
    public TMP_Text dayText;

    [Header("Bool Check")]
    [Tooltip("Check if player decide to open the store.")]
    public bool IsStoreOpen = false;
    [Tooltip("Check if The Day Has Started(After finish sleeping sequence).")]
    public bool IsDayStart = false;

    [Header("UI Reference")]
    [SerializeField] private RectTransform clockHandTransform;


    [Header("Sleep")]
    public Transform sleepTf;
    public Image sleepButton;

    private StoreState storeStage;
    private void Start()
    {
        timeMultiplier = 24f / dayLengthInSeconds;
        timeMultiplierShopOpen = timeMultiplier;
        timeMultiplierShopClose = timeMultiplier / 2f;        
    }

    private void Update()
    {
        if(currentDay > 7)
        {
            if(InventoryHolder.instance.Money < 10000)
            {
                Application.Quit();
            }
        }

        if (IsDayStart) AdvanceTime();
        UpdateClockUI();

        OnSleep();
    }

    private void AdvanceTime()
    {
        if (currTime >= 24f)
        {
            currTime = 0f;
            currentDay++;
            IsStoreOpen = false;

            // Change Day Sequence
        }

        switch (storeStage)
        {
            case StoreState.Preparation:
                currTime += Time.deltaTime * timeMultiplierShopClose;

                if (currTime >= shopOpenTime)
                {
                    TryOpenStore();
                }
                break;

            case StoreState.OpenStore:
                currTime += Time.deltaTime * timeMultiplierShopOpen;

                if (currTime >= shopCloseTime)
                {
                    TryCloseStore();
                }
                break;

            case StoreState.CloseStore:
                currTime += Time.deltaTime * timeMultiplierShopClose;
                break;

        }

    }

    public void TryOpenStore()
    {
        if (storeStage == StoreState.Preparation)
        {
            currTime = shopOpenTime;
            storeStage = StoreState.OpenStore;
            IsStoreOpen = true;
        }
    }

    public void TryCloseStore()
    {
        if (storeStage == StoreState.OpenStore)
        {
            currTime = shopCloseTime;
            storeStage = StoreState.CloseStore;
            IsStoreOpen = false;
        }
    }

    private void UpdateClockUI()
    {
        float angle = currTime * -15f;
        if(clockHandTransform != null)
        {
            clockHandTransform.localEulerAngles = new Vector3(0f, 0f, angle);
        }
        else
        {
            clockHandTransform = GameObject.FindWithTag("ClockHand").GetComponent<RectTransform>();
        }
    }


    public float GetCurTime()
    {
        return currTime;
    }

    public bool CanOpenShopBeforeTime()
    {
        if(currTime <= shopOpenTime)
        {
            return true;
        }

        return false;
    }

    public void OnSleep()
    {
        if(dayText == null) dayText = GameObject.FindWithTag("DayText").GetComponent<TMP_Text>();
        
        dayText.text = $"Day {currentDay}";

        if (sleepButton == null) sleepButton = GameObject.FindWithTag("SleepButton").GetComponent<Image>();        
        if(sleepTf == null) sleepTf = GameObject.FindWithTag("SleepTF").GetComponent<Transform>();
        Transform playerTf = GameObject.FindWithTag("Player").GetComponent<Transform>();
        float distance = Vector3.Distance(playerTf.position, sleepTf.position);

        if (distance < 0.5f)
        {
            sleepButton.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                TransitionManager.Instance.PlayTransition(2f);

                currentDay++;
                currTime = 0;
                IsStoreOpen = false;
            }
        }
        else
        {
            sleepButton.gameObject.SetActive(false);
        }
    }

}

public enum StoreState {Preparation, OpenStore, CloseStore}