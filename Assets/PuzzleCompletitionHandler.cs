using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class PuzzleCompleteHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MemorySocketGroup memorySocketGroup;

    [Header("UI")]
    [SerializeField] private GameObject completionUI;     
    [SerializeField] private TextMeshProUGUI completionText;
    [SerializeField] private string completionMessage = "Puzzle Complete!";

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip completionSound;
    [SerializeField] private AudioClip failSound;         

    [Header("Timing")]
    [SerializeField] private float uiDelay = 0.5f;        
    [SerializeField] private float uiDuration = 3f;        

    [Header("Events")]
    public UnityEvent onPuzzleComplete;  
    public UnityEvent onPuzzleBroken;

    void Start()
    {
        if (completionUI != null)
            completionUI.SetActive(false);

        memorySocketGroup.onAllMatch.AddListener(OnPuzzleComplete);
        memorySocketGroup.onMatchBroken.AddListener(OnPuzzleBroken);
    }

    void OnDestroy()
    {
        memorySocketGroup.onAllMatch.RemoveListener(OnPuzzleComplete);
        memorySocketGroup.onMatchBroken.RemoveListener(OnPuzzleBroken);
    }

    private void OnPuzzleComplete()
    {
        Debug.Log("[Puzzle] Complete!");
        PlaySound(completionSound);
        StartCoroutine(ShowUI());
        onPuzzleComplete.Invoke();
    }

    private void OnPuzzleBroken()
    {
        Debug.Log("[Puzzle] Match broken.");
        PlaySound(failSound);
        HideUI();
        onPuzzleBroken.Invoke();
    }

    private IEnumerator ShowUI()
    {
        yield return new WaitForSeconds(uiDelay);

        if (completionUI != null)
            completionUI.SetActive(true);

        if (completionText != null)
            completionText.text = completionMessage;

        //if (uiDuration > 0f)
        //{
        //    yield return new WaitForSeconds(uiDuration);
        //    HideUI();
        //}
    }

    private void HideUI()
    {
        if (completionUI != null)
            completionUI.SetActive(false);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}