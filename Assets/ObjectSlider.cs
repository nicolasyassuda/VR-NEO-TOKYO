using System.Collections;
using UnityEngine;

public class ObjectSlider : MonoBehaviour
{
    [Header("Positions")]
    [SerializeField] private Transform targetPosition;  // where it slides TO
    // Used if no targetPosition assigned
    [SerializeField] private Vector3 slideOffset = new Vector3(2f, 0f, 0f);

    [Header("Settings")]
    [SerializeField] private float slideDuration = 1f;  // seconds to complete slide
    [SerializeField] private AnimationCurve slideCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private bool _isOpen = false;
    private Coroutine _currentSlide;

    void Start()
    {
        _startPosition = transform.position;
        _endPosition = targetPosition != null
            ? targetPosition.position
            : _startPosition + slideOffset;
    }

    public void SlideTo(Vector3 worldPosition)
    {
        _endPosition = worldPosition;
        _isOpen = true;
        if (_currentSlide != null) StopCoroutine(_currentSlide);
        _currentSlide = StartCoroutine(DoSlide(_endPosition));
    }

    public void SlideTo(float x, float y, float z) => SlideTo(new Vector3(x, y, z));

    public void Slide()
    {
        _isOpen = !_isOpen;
        if (_currentSlide != null) StopCoroutine(_currentSlide);
        _currentSlide = StartCoroutine(DoSlide(_isOpen ? _endPosition : _startPosition));
    }

    // Explicitly open or close
    public void SlideOpen()
    {
        if (_isOpen) return;
        _isOpen = true;
        if (_currentSlide != null) StopCoroutine(_currentSlide);
        _currentSlide = StartCoroutine(DoSlide(_endPosition));
    }

    public void SlideClose()
    {
        if (!_isOpen) return;
        _isOpen = false;
        if (_currentSlide != null) StopCoroutine(_currentSlide);
        _currentSlide = StartCoroutine(DoSlide(_startPosition));
    }

    private IEnumerator DoSlide(Vector3 target)
    {
        Vector3 origin = transform.position;
        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = slideCurve.Evaluate(elapsed / slideDuration);
            transform.position = Vector3.Lerp(origin, target, t);
            yield return null;
        }

        transform.position = target;
        _currentSlide = null;
    }

    public bool IsOpen => _isOpen;
}