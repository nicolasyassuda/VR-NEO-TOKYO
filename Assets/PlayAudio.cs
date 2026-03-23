using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayAudio : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip defaultClip;

    [Header("Settings")]
    [SerializeField] private float volume = 1f;
    [SerializeField] private bool playOnStart = false;

    private AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.volume = volume;
    }

    void Start()
    {
        if (playOnStart) Play();
    }

    // Play the default clip
    public void Play()
    {
        if (defaultClip == null) return;
        _audioSource.PlayOneShot(defaultClip, volume);
    }

    // Play a specific clip
    public void PlayClip(AudioClip clip)
    {
        if (clip == null) return;
        _audioSource.PlayOneShot(clip, volume);
    }

    // Play and loop
    public void PlayLoop()
    {
        if (defaultClip == null) return;
        _audioSource.clip = defaultClip;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    // Stop any playing audio
    public void Stop()
    {
        _audioSource.Stop();
    }

    // Pause
    public void Pause()
    {
        _audioSource.Pause();
    }

    // Resume
    public void Resume()
    {
        _audioSource.UnPause();
    }

    // Change volume at runtime (0 to 1)
    public void SetVolume(float val)
    {
        volume = Mathf.Clamp01(val);
        _audioSource.volume = volume;
    }
}