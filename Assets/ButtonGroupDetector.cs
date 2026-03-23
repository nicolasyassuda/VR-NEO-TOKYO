using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonGroupDetector : MonoBehaviour
{
    [SerializeField] private PhysicalToggleButton[] allButtons;


    public UnityEvent onAllPressed; 
    public UnityEvent onAnyReleased; 

    private bool _allPressedLastFrame = false;

    void Update()
    {
        if (allButtons == null) return;

        bool allPressed = AreRequiredPressed();

        if (allPressed && !_allPressedLastFrame)
        {
            Debug.Log($"[ButtonGroup] Required combination complete!");
            onAllPressed.Invoke();
        }
        else if (!allPressed && _allPressedLastFrame)
        {
            Debug.Log($"[ButtonGroup] Combination broken.");
            onAnyReleased.Invoke();
        }

        _allPressedLastFrame = allPressed;
    }

    private bool AreRequiredPressed()
    {
        for (int i = 0; i < 3; i++)
        {
            switch (i)
            {
                case 0:
                    if (allButtons[i].IsToggled) return false;
                    break;
                case 1:
                    if (!allButtons[i].IsToggled) return false;
                    break;
                case 2:
                    if (!allButtons[i].IsToggled) return false;
                    break;
                default:
                    return false;
            }
        }
        return true;
    }
    public bool IsGroupComplete() => AreRequiredPressed();
}