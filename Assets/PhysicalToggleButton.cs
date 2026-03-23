using UnityEngine;
using UnityEngine.Events;

public class PhysicalToggleButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody buttonRigidbody;

    [Header("Settings")]
    [SerializeField] private float maxPressDistance = 0.02f;
    [SerializeField] private float returnSpeed = 800f;
    [SerializeField] private float damper = 60f;
    [SerializeField] private float pressThreshold = 0.015f;
    [SerializeField] private float resetCooldown = 0.5f;

    [Header("Events")]
    public UnityEvent onPressed;   // fires when toggled ON
    public UnityEvent onReleased;  // fires when toggled OFF

    private Vector3 _restPosition;
    private Vector3 _lockedPosition;
    private float _bottomLimit;

    public bool IsToggled => _toggledOn;
    private bool _toggledOn = false;   // is the button currently locked down?
    private bool _isPhysicallyDown = false;  // is the button physically past threshold?
    private bool _cooldown = false;

    void Start()
    {
        _restPosition = buttonRigidbody.transform.localPosition;
        _bottomLimit = _restPosition.y - maxPressDistance;
        _lockedPosition = new Vector3(_restPosition.x, _bottomLimit, _restPosition.z);

        buttonRigidbody.useGravity = false;

    }

    void FixedUpdate()
    {
        float currentY = buttonRigidbody.transform.localPosition.y;

        // --- Hard clamp position ---
        if (currentY > _restPosition.y)
        {
            SetY(_restPosition.y);
            currentY = _restPosition.y;
        }
        else if (currentY < _bottomLimit)
        {
            SetY(_bottomLimit);
            currentY = _bottomLimit;
        }

        float travel = _restPosition.y - currentY;

        // --- Detect physical press crossing threshold ---
        if (!_isPhysicallyDown && travel >= pressThreshold)
        {
            _isPhysicallyDown = true;
            OnThresholdCrossed();
        }
        else if (_isPhysicallyDown && travel < pressThreshold * 0.5f)
        {
            _isPhysicallyDown = false;
        }

        // --- Movement behavior depends on toggle state ---
        if (_toggledOn)
        {
            // Lock button at bottom position
            SetY(_bottomLimit);
        }
        else
        {
            // Spring back to rest
            float displacement = _restPosition.y - currentY;
            float springForce = (displacement * returnSpeed) - (buttonRigidbody.linearVelocity.y * damper);
            if (currentY < _restPosition.y)
                buttonRigidbody.AddForce(Vector3.up * springForce, ForceMode.Force);
        }
    }

    private void OnThresholdCrossed()
    {
        if (_cooldown) return;

        _toggledOn = !_toggledOn;

        if (_toggledOn)
            onPressed.Invoke();   // toggled ON  → button stays down
        else
            onReleased.Invoke();  // toggled OFF → button springs back up

        StartCoroutine(StartCooldown());
    }

    private void SetY(float y)
    {
        buttonRigidbody.transform.localPosition = new Vector3(
            _restPosition.x, y, _restPosition.z);
        buttonRigidbody.linearVelocity = Vector3.zero;
    }

    private System.Collections.IEnumerator StartCooldown()
    {
        _cooldown = true;
        yield return new WaitForSeconds(resetCooldown);
        _cooldown = false;
    }
}