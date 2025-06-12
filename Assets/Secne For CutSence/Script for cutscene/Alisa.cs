using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alisa : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public bool MainGirl;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
       

    }

    // Update is called once per frame
    void Update()
    {
        MainGirl = PlayerPrefs.GetInt("Endcutscene2", 0) == 1;
        if (MainGirl == true)
        {
            spriteRenderer.enabled = true;
        }
        else 
        {
            spriteRenderer.enabled = false;
        }
    }
}
