/*using Firebase;
using Firebase.Auth;
using Firebase.Extensions;*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public Text text;
    public float score;


    [Header("Register")]
    public InputField reg_email;
    public InputField reg_password;
    public Text Log;

   // private FirebaseAuth auth;
    
    private void Start()
    {
        /*
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                Debug.Log("Firebase Ready");
                auth = FirebaseAuth.DefaultInstance;
            }
        });
        */
    }

    void Update()
    {
        score += Time.deltaTime;
        text.text = score.ToString();
    }

    public void Register() //Register to Firebase
    {
        if(reg_email== null)
        {
            return;
        }
        if(reg_password== null)
        {
            return;
        }

        /*auth.CreateUserWithEmailAndPasswordAsync(reg_email.text, reg_password.text).ContinueWithOnMainThread(task=>
        {
            if(task.IsCompleted && !task.IsFaulted)
            {
                AuthResult auth = task.Result;
                Debug.Log("Success");
                Log.text = ("Register Success" + auth.User.UserId);

            }
            else
            {
                Log.text = "Error";
            }
        });
        */

    }

    public void SaveGame()
    {
        SaveGame saveGame = new SaveGame();
        saveGame.score = score;
        saveGame.name = "P1";
        string saveStr = JsonUtility.ToJson(saveGame); // Convert everything in "savegame" to stringStr
        PlayerPrefs.SetString("MySave", saveStr);
    }

    public void LoadGame()
    {
        string LoadStr = PlayerPrefs.GetString("MySave", "NewGame");
        if(LoadStr != "NewGame")
        {
            SaveGame loadGame = (SaveGame)JsonUtility.FromJson<SaveGame>(LoadStr);
            score= loadGame.score;

        }
    }
}

[SerializeField] //Convert Everyting to String
public class SaveGame
{
    // Declare what to save here

    public float score;
    public string name;
}
