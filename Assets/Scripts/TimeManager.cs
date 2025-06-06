using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    [Header("Time Setting")]
    [Tooltip("How many real-time seconds a full day lasts.")]
    [SerializeField] private float dayLengthInSeconds = 600f; // Player will have to sleep so spare 8 hrs in-game for it
    [SerializeField] private float currTime = 0f;


    [Header("Day Progression")]
    public int currentDay = 1;
    private float timeMultiplier;
    private float timeMultiplierShopClose; // Time When Shop Close run slowly
    private float timeMultiplierShopOpen; // Time When Shop Open run faster

    [Header("Bool Check")]
    [Tooltip("Check if player decide to open the store.")]
    public bool IsStoreOpen = false;
    [Tooltip("Check if The Day Has Started(After finish sleeping sequence).")]
    public bool IsDayStart = false;

    private StoreState storeStage;
    private void Start()
    {
        timeMultiplier = 24f / dayLengthInSeconds;
        timeMultiplierShopOpen = timeMultiplier;
        timeMultiplierShopClose = timeMultiplier / 2f;        
    }

    private void Update()
    {
        if (IsDayStart) AdvanceTime();
    }

    private void AdvanceTime()
    {
        if (currTime >= 24f)
        {
            currTime = 0f;
            currentDay++;

            // Change Day Sequence
        }
        
        switch (storeStage)
        {
            case StoreState.Preparation:
                currTime += Time.deltaTime * timeMultiplierShopClose;

                if (/*!manualStoreControl && */currTime >= 24f * 0.4f)
                {
                    TryOpenStore();
                }
                break;

            case StoreState.OpenStore:
                currTime += Time.deltaTime * timeMultiplierShopOpen;

                if (/*!manualStoreControl && */currTime >= 24f * 0.8f)
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
            currTime = 24f * 0.4f;
            storeStage = StoreState.OpenStore;
            IsStoreOpen = true;
        }
    }

    public void TryCloseStore()
    {
        if (storeStage == StoreState.OpenStore)
        {
            currTime = 24f * 0.8f;
            storeStage = StoreState.CloseStore;
            IsStoreOpen = false;
        }
    }
}

public enum StoreState {Preparation, OpenStore, CloseStore}
