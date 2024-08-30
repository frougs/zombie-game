using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopupTextScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI headerText;
    [SerializeField] TextMeshProUGUI bodyText;
    [SerializeField] CanvasGroup canvasGroup;
    public void Message(string header, string body, float duration){
        if (!string.IsNullOrEmpty(header)){
            headerText.text = header;
        }
        if (!string.IsNullOrEmpty(body)){
            bodyText.text = body;
        }
        StartCoroutine(FadeOutText(duration));
    }
    private IEnumerator FadeOutText(float fadeDuration)
    {
        if (canvasGroup == null)
        {
            yield break;
        }
        canvasGroup.alpha = 1.0f;
        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            yield return null;
        }
        headerText.text = "";
        bodyText.text = "";
        canvasGroup.alpha = 0f;  // Ensure the alpha is exactly 0 after the fade
    }
}
