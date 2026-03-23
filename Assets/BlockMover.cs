using UnityEngine;

public class BlockMover : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PhysicalToggleButton button;

    [Header("Positions")]
    [SerializeField] private Transform targetPosition;

    [SerializeField] private Vector3 moveOffset = new Vector3(0, 2f, 0);

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 2f;

    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private bool _lastButtonState = false;

    void Start()
    {
        _startPosition = transform.position;
        _endPosition = targetPosition != null
            ? targetPosition.position
            : _startPosition + moveOffset;
    }

    void Update()
    {
        if (button == null) return;

        bool isOn = button.IsToggled;

        if (isOn != _lastButtonState)
        {
            Debug.Log($"[BlockMover] Moving {gameObject.name} → {(isOn ? "END" : "START")}");
            _lastButtonState = isOn;
        }

        Vector3 target = isOn ? _endPosition : _startPosition;
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * moveSpeed);
    }
}