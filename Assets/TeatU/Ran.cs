using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ran : MonoBehaviour
{
    float maxTime = 600; //10 mins
    public List<float> spawnTimes = new List<float>();
    public int spawnCount;  //How Many Monsters to Create
    [SerializeField]float gameTime;
    public bool clearGameTime;

    void Start()
    {
        CreateSpawnList();
        StartCoroutine(OnGameLoop());
    }

    void Update()
    {
       // gameTime = Mathf.Round(Time.time);
        if(gameTime >= maxTime)
        {
            gameTime= 0;
            spawnTimes.Clear();
            spawnCount += Random.Range(2, 5);
            CreateSpawnList();
        }
        foreach (float spawnTime in spawnTimes)
        {
            if(Mathf.Approximately(gameTime,spawnTime))
            {
                Debug.Log("Spawn");
            }
        }
        if(clearGameTime) gameTime= 0;

    }
    IEnumerator OnGameLoop()
    {
        while(gameTime < maxTime)
        {
            yield return new WaitForSecondsRealtime(1f);
            gameTime += 1;

        }
    }
    void CreateSpawnList() // Funtions Create List of Spawn Time
    {
        float lastTime = 0f;
        for (int i = 0; i < spawnCount; i++)
        {
            float newTime = OnRanSpawn(lastTime, maxTime);
            spawnTimes.Add(newTime);
            lastTime = newTime;

        }
    }
    float OnRanSpawn (float lateTime, float maxTime)
    {
        float rannum = Random.Range(0, maxTime);
        if(Mathf.Abs(rannum - lateTime) < 30)
        {
            rannum = Random.Range(0, maxTime);
        }
        return Mathf.Round(rannum);
    }
}