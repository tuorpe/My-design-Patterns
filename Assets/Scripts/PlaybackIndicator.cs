using UnityEngine;

public class PlaybackIndicator : MonoBehaviour
{
    public static PlaybackIndicator instance;
    private TMPro.TextMeshProUGUI text;
    private float blinkPeriod = 0.5f; // Time in seconds for one blink cycle
    private float blinkTimer = 0f; // Timer to track the blinking
    bool isBlinking = false;
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        text = GetComponent<TMPro.TextMeshProUGUI>();
    }

    private void FixedUpdate()
    {
        if(!isBlinking)
            return;
        if(blinkTimer > blinkPeriod)
        {
            // Reset the timer
            blinkTimer = 0f;
            // Toggle the visibility of the text
            text.enabled = !text.enabled;
        }
        else
        {
            // Increment the timer
            blinkTimer += Time.deltaTime;
        }
    }
    
    public void ShowMessage(string message, bool blink = false)
    {
        isBlinking = blink;
        text.enabled = true;
        blinkTimer = 0f;
        text.text = message;
    }
}
