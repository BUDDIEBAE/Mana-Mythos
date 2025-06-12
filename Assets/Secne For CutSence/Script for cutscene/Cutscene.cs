using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public bool showui;
    public bool lastcutscene;
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        showui = PlayerPrefs.GetInt("Endcutscene", 0) == 1;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (showui == true)
        {
            canvas.enabled = true;
            if (lastcutscene == true)
            {
                PlayerPrefs.SetInt("Endcutscene2", 1);
            }
        }
        else
        {
            canvas.enabled=false;
        }

    }
}
