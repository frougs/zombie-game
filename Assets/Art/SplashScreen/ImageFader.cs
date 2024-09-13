using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Required for scene management
using System.Collections;

public class ImageFader : MonoBehaviour
{
    public Image imageToFade; // Reference to the UI Image component
    public float fadeDuration = 1f; // Duration of the fade-out effect

    private void Start()
    {
        // Ensure the image fades out when the scene loads
        if (imageToFade != null)
        {
            StartCoroutine(FadeOutImage());
        }
    }

    public void StartFadeOut()
    {
        // Start the fade-out coroutine when needed
        if (imageToFade != null)
        {
            StartCoroutine(FadeOutImage());
        }
    }

    private IEnumerator FadeOutImage()
    {
        if (imageToFade == null)
        {
            Debug.LogWarning("ImageToFade is not assigned.");
            yield break;
        }

        Color startColor = imageToFade.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(startColor.a, endColor.a, elapsedTime / fadeDuration);
            imageToFade.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final alpha is set to 0
        imageToFade.color = endColor;
        //Destroy(this.gameObject);

        // Optionally, reload the scene when fade-out is complete
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
