using UnityEngine;
using UnityEngine.InputSystem;
public class InputTest : MonoBehaviour
{
    public InputActionProperty _inputActionSelect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float value = _inputActionSelect.action.ReadValue<float>();

        Debug.Log(value);
    }
}
