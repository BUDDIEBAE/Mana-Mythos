using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MainMenuActions : MonoBehaviour
{
    [SerializeField] private List<Image> _highlightImages = new List<Image>();

    [SerializeField] private Scene _sceneToLoad;

    [SerializeField] private Image _progressBar;

    [SerializeField] private TMP_Text _progressText;

    [SerializeField] private GameObject _LoadCanvas;


    private void Start()
    {
        foreach (var image in _highlightImages)
        {
            if(image.IsActive())
            {
                image.color = Color.clear;
            }
        }
    }

    public void OnStartClick(Scene scene)
    {
        _LoadCanvas.SetActive(true);

        StartCoroutine(LoadingScene(_sceneToLoad));
    }

    private IEnumerator LoadingScene(Scene scene)
    {
        AsyncOperation loadAsyncScene = SceneManager.LoadSceneAsync(scene.name);
        loadAsyncScene.allowSceneActivation = false;


        while (loadAsyncScene.isDone)
        {
            float progress = Mathf.Clamp01(loadAsyncScene.progress / 0.9f);
            _progressBar.fillAmount = progress;

            if(loadAsyncScene.progress >= 0.9f)
            {
                _progressText.text = "";

                if (Input.anyKeyDown)
                {

                    loadAsyncScene.allowSceneActivation = true;
                }
            }


            yield return null;
        }
    }
}
