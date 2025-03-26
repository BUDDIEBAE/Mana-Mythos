using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;

public class MiniGameController : MonoBehaviour
{
    [Header("Parameter")]
    public float _minigameSpeed;


    [Header("References")]
    public CraftingSystem _craftingSystem;
    public RectTransform _gaugeBar;
    public Image _fill;

    [Header("Prefabs")]
    public RectTransform _greenBarPrefab;

    private RectTransform[] _greenBars;
    private Coroutine _minigameRoutine;
    private bool _isRunning = false;
    private float _gaugeWidth;
    private int _barsRemaining;
    public bool _allHitActive => _barsRemaining == 0;

    void Start()
    {
        _gaugeWidth = _gaugeBar.rect.width;
    }

    // Sets up green bars based on item count
    public void SetUpGreenBars()
    {
        ClearGreenBar();

        int itemCount = 0;
        foreach (Slot slot in _craftingSystem._slots)
        {
            if (slot._itemdata != null) itemCount++;
        }

        _greenBars = new RectTransform[itemCount];
        _barsRemaining = itemCount;

        List<float> usedPositions = new List<float>();
        float minDistance = 20f; // Minimum distance between bars

        for (int i = 0; i < itemCount; i++)
        {
            RectTransform greenBar = Instantiate(_greenBarPrefab, _gaugeBar);

            float randomX;
            int attempts = 0;
            bool positionValid;

            do
            {
                positionValid = true;
                randomX = Random.Range(20f, 90f) / 100f * _gaugeWidth - (_gaugeWidth / 2);

                // Check against existing positions
                foreach (float pos in usedPositions)
                {
                    if (Mathf.Abs(pos - randomX) < minDistance)
                    {
                        positionValid = false;
                        break;
                    }
                }

                attempts++;
                if (attempts > 50) break; // Prevent infinite loops

            } while (!positionValid);

            greenBar.anchoredPosition = new Vector2(randomX, 0);
            _greenBars[i] = greenBar;
            usedPositions.Add(randomX);
        }
    }

    // Clears all existing green bars
    private void ClearGreenBar()
    {
        if (_greenBars != null)
        {
            foreach (var bar in _greenBars)
            {
                if (bar != null)
                {
                    Destroy(bar.gameObject);
                }
            }
        }
    }

    // Starts the mini-game when the player presses a button
    public void StartMiniGame()
    {
        if (_isRunning) return;
        Debug.Log("Mini-game Started");

        //SetUpGreenBars();
        _isRunning = true;
        _minigameRoutine = StartCoroutine(RunMiniGame());
    }

    // Stops the mini-game and resets the UI
    public void StopMiniGame()
    {
        if (!_isRunning) return;
        Debug.Log("Minigame-Stopped");
        _isRunning = false;
        _fill.fillAmount = 0f;

        if (_minigameRoutine != null)
        {
            StopCoroutine(_minigameRoutine);
            _minigameRoutine = null;
        }
        ClearGreenBar();
    }

    // Runs the mini-game logic in a coroutine
    private IEnumerator RunMiniGame()
    {
        while (_isRunning)
        {
            _fill.fillAmount += _minigameSpeed * Time.deltaTime; //Gauge speed
            if (_fill.fillAmount >= 1f) _fill.fillAmount = 0f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                CheckHits();
            }

            if (_allHitActive)
            {
                Debug.Log("All bars hit! Crafting successful.");
                StopMiniGame();
                _craftingSystem.CompleteCrafting();
            }

            yield return null;
        }
    }

    // Checks if the player successfully hits a green bar
    private void CheckHits()
    {
        foreach (var bar in _greenBars)
        {
            if (bar == null) continue;

            float barPosX = bar.anchoredPosition.x / _gaugeWidth + 0.5f;
            if (Mathf.Abs(_fill.fillAmount - barPosX) < 0.05f)
            {
                Debug.Log("Hit!");

                //CameraShaker.Instance.ShakeOnce(0.8f, 10f, 0f, 0.3f); // Called an camerashake function here
                UIEffectController.instance.ShakeHandle(0.3f, 8f, 1f);

                Destroy(bar.gameObject);
                _barsRemaining--;
            }
        }
    }

}
