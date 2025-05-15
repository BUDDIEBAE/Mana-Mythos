using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuActions : MonoBehaviour
{
    [SerializeField] private List<Image> _highlightImages = new List<Image>();



    [SerializeField] private Image _progressBar;

    [SerializeField] private TMP_Text _progressText;

    [SerializeField] private GameObject _LoadCanvas;


    private void Start()
    {
        foreach (var image in _highlightImages)
        {
            if (image.IsActive())
            {
                image.color = Color.clear;
            }
        }
    }

    public void OnStartClick(string sceneName)
    {
        Debug.Log("Clicked");
        _LoadCanvas.SetActive(true);

        StartCoroutine(LoadingScene(sceneName));
    }

    private IEnumerator LoadingScene(string sceneName)
    {
        AsyncOperation loadAsyncScene = SceneManager.LoadSceneAsync(sceneName);
        loadAsyncScene.allowSceneActivation = false;



        while (!loadAsyncScene.isDone)
        {
            float progress = Mathf.Clamp01(loadAsyncScene.progress / 0.9f);
            _progressBar.fillAmount = progress;

            if (loadAsyncScene.progress >= 0.9f)
            {
                _progressText.text = "Press Any Key To Continue";

                if (Input.anyKeyDown)
                {

                    loadAsyncScene.allowSceneActivation = true;
                }
            }


            yield return null;
        }
    }


    public void OnExit()
    {
        #if UNITY_EDITOR
                EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif

    }
}
