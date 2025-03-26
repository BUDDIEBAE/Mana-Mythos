using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffectController : Singleton<UIEffectController>
{
    [SerializeField] private GameObject _craftUI;


    public void ShakeHandle(float duration, float magnitude, float fadeTime)
    {
        StartCoroutine(UIShake(duration, magnitude, fadeTime));
    }

    private IEnumerator UIShake(float duration, float magnitude, float fadeTime)
    {
        Vector3 originalPos = _craftUI.transform.position;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // Calculate the current shake strength based on fade-in and fade-out
            float progress = elapsed / duration; // 0 to 1 over the shake duration
            float fadeMultiplier = 1f;

            if (elapsed < fadeTime)
            {
                // Fade-in effect (first part of the shake)
                fadeMultiplier = Mathf.Lerp(0, 1, elapsed / fadeTime);
            }
            else if (elapsed > duration - fadeTime)
            {
                // Fade-out effect (last part of the shake)
                fadeMultiplier = Mathf.Lerp(1, 0, (elapsed - (duration - fadeTime)) / fadeTime);
            }
            float currentMagnitude = magnitude * fadeMultiplier;

            float x = UnityEngine.Random.Range(-0.2f, 0.2f) * currentMagnitude;
            float y = UnityEngine.Random.Range(-5f, 5f) * currentMagnitude;

            _craftUI.transform.position = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        _craftUI.transform.position = originalPos; // Reset position
    }


}

