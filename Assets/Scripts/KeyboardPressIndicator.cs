using UnityEngine;

public class KeyboardPressIndicator : MonoBehaviour
{
    private float showTimeout = 0.25f; // Time in seconds to show the indicator
    private float showTimer = 0f; // Timer to track how long the indicator has been shown
    private TMPro.TextMeshProUGUI text;
    
    private void Start()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
    }
   
    private void FixedUpdate()
    {
        if (showTimer > showTimeout)
        {
            // Hide the indicator after the timeout
            text.text = "";
        }
        else
        {
            // Increment the timer
            showTimer += Time.deltaTime;
        }
    }
    
    public void ShowMessage(string message, float duration = 0.25f)
    {
        // If the text is already showing, don't overwrite it until it has been cleared
        if(text.text != "")
            return;
        // Reset the timer
        showTimer = 0f;
        // Set the text of the TextMeshProUGUI
        text.text = message;
    }
}
