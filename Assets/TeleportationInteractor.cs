using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
public class TeleportationInteractor : MonoBehaviour
{
    public XRRayInteractor _teleportInteractor;
    public InputActionProperty _teleportInteractionAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _teleportInteractor.gameObject.SetActive(false);

        _teleportInteractionAction.action.performed += Action_performed;
    }

    private void Action_performed(InputAction.CallbackContext obj)
    {
        _teleportInteractor.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (_teleportInteractionAction.action.WasReleasedThisFrame())
        {
            _teleportInteractor.gameObject.SetActive(false);
        }
    }
}
