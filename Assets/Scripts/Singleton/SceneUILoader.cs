using State_Machine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SceneUILoader : MonoBehaviour
{
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject exitButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (startButton)
        {
            startButton.GetComponent<Button>().onClick.AddListener(GameManagerSM.Instance.StartGame);
            AddButtonHoverListener(startButton);
        }

        if (exitButton)
        {
            exitButton.GetComponent<Button>().onClick.AddListener(GameManagerSM.Instance.OnExitGame);
            AddButtonHoverListener(exitButton);
        }
    }

    private static void AddButtonHoverListener(GameObject target)
    {
        var buttonTrigger = target.GetComponent<EventTrigger>();
        if (!buttonTrigger)
            target.AddComponent<EventTrigger>();
        else
        {
            var entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            entry.callback.AddListener((eventData) => { AudioManager.Instance.PlayButtonHoverSound(); });
            buttonTrigger.triggers.Add(entry);
        }
        
    }
}
