using UnityEngine;
using UnityEngine.InputSystem;
public class AnimationInputUpdate : MonoBehaviour
{
    public InputActionProperty _triggerValue;
    public InputActionProperty _gripValue;

    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float triggerValue = _triggerValue.action.ReadValue<float>();
        float gripValue = _gripValue.action.ReadValue<float>();
        animator.SetFloat("Trigger", triggerValue);
        animator.SetFloat("Grip", gripValue);
    }
}
