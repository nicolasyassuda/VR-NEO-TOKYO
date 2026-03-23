using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(XRBaseInteractable))]
public class TouchAudioTrigger : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip touchSound;

    [Header("Settings")]
    [SerializeField] private float cooldown = 1f;  // prevents spamming

    private AudioSource _audioSource;
    private XRBaseInteractable _interactable;
    private float _lastPlayTime = -999f;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;

        _interactable = GetComponent<XRBaseInteractable>();
        _interactable.hoverEntered.AddListener(OnTouched);
    }

    void OnDestroy()
    {
        _interactable.hoverEntered.RemoveListener(OnTouched);
    }

    private void OnTouched(HoverEnterEventArgs args)
    {
        // Only play if cooldown has passed
        if (Time.time - _lastPlayTime < cooldown) return;

        // Only react to Direct Interactor, ignore Ray
        if (args.interactorObject is XRDirectInteractor)
        {
            PlaySound();
        }
    }

    private void PlaySound()
    {
        if (touchSound == null) return;
        _audioSource.PlayOneShot(touchSound);
        _lastPlayTime = Time.time;
        Debug.Log($"[TouchAudio] Playing sound on {gameObject.name}");
    }
}