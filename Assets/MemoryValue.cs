using UnityEngine;
using TMPro;

public class MemoryValue : MonoBehaviour
{
    [Header("Value")]
    public int memoryValue;

    [Header("References")]
    [SerializeField] private TextMeshPro label;  // drag your TMP here

    void Start() => UpdateLabel();

    // Call this if you change memoryValue at runtime
    public void SetValue(int value)
    {
        memoryValue = value;
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        if (label != null)
            label.text = memoryValue.ToString();
    }

    // Auto-update in editor when value changes
    void OnValidate() => UpdateLabel();
}