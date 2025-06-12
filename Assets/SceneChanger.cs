using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public float changeTime = 5f;
    public string sceneName;

    public Camera cutsceneCamera;
    public Camera playerCamera;

    private bool cutscenePlayed;

    void Start()
    {
        PlayerPrefs.SetInt("Endcutscene", 0);
        PlayerPrefs.SetInt("Endcutscene2", 0);
        // เช็คว่ามีการเล่น Cutscene ไปแล้วหรือยัง
        cutscenePlayed = PlayerPrefs.GetInt("CutscenePlayed", 0) == 1;

        if (!cutscenePlayed)
        {
            PlayCutscene();
        }
        else
        {
            SkipCutscene();
        }
    }

    void PlayCutscene()
    {
        if (cutsceneCamera != null && playerCamera != null)
        {
            cutsceneCamera.enabled = true;
            playerCamera.enabled = false;
        }

        PlayerPrefs.SetInt("CutscenePlayed", 1);
        PlayerPrefs.Save();

        // เรียกเปลี่ยนฉากหลังจากจบ Cutscene
        Invoke("ChangeScene", changeTime);
    }

    void SkipCutscene()
    {
        if (cutsceneCamera != null && playerCamera != null)
        {
            cutsceneCamera.enabled = false;
            playerCamera.enabled = true;
        }

        // เรียกเปลี่ยนฉากโดยไม่ต้องรอ Cutscene
        Invoke("ChangeScene", changeTime);
    }

    void ChangeScene()
    {
        PlayerPrefs.SetInt("Endcutscene", 1);
        SceneManager.LoadScene(sceneName);
    }
}
