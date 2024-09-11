using System.Collections;
using UnityEngine;
using TMPro;

public class TypingEffect : MonoBehaviour
{
    public TMP_Text displayText; // The TextMeshPro UI element where the string will be displayed
    public string inputString; // The string to be displayed
    public float updateInterval = 0.1f; // Interval between updates

    private void Start()
    {
        if (displayText == null)
        {
            Debug.LogError("Display Text (TMP) is not assigned!");
            return;
        }

        StartCoroutine(RandomlyRevealString(inputString));
    }

    private IEnumerator RandomlyRevealString(string str)
    {
        char[] charArray = str.ToCharArray();
        char[] displayArray = new char[charArray.Length];
        
        // Initialize displayArray with spaces
        for (int i = 0; i < displayArray.Length; i++)
        {
            displayArray[i] = ' ';
        }

        // Create a list of indices to randomly replace characters
        System.Collections.Generic.List<int> indices = new System.Collections.Generic.List<int>();
        for (int i = 0; i < charArray.Length; i++)
        {
            indices.Add(i);
        }

        // Continue until all characters are revealed
        while (new string(displayArray) != str)
        {
            int randomIndex = Random.Range(0, indices.Count);
            int charIndex = indices[randomIndex];
            
            if (displayArray[charIndex] == ' ') // Only replace spaces
            {
                displayArray[charIndex] = charArray[charIndex];
                indices.RemoveAt(randomIndex); // Remove index to avoid re-selection
            }

            displayText.text = new string(displayArray);
            
            // Wait for the specified interval before the next update
            yield return new WaitForSeconds(updateInterval);
        }
    }
}
