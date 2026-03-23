using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRestarter : MonoBehaviour
{
    [Header("Scene to Load")]
    [Tooltip("Leave -1 to restart the current scene")]
    [SerializeField] private int sceneBuildIndex = -1;
    [Tooltip("Optional: use scene name instead of build index")]
    [SerializeField] private string sceneName = "";

    public void RestartScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
            SceneManager.LoadScene(sceneName);
        else if (sceneBuildIndex >= 0)
            SceneManager.LoadScene(sceneBuildIndex);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // fallback: current scene
    }

    public void RestartSceneDelayed(float delay)
    {
        StartCoroutine(DelayedRestart(delay));
    }

    private System.Collections.IEnumerator DelayedRestart(float delay)
    {
        yield return new WaitForSeconds(delay);
        RestartScene();
    }
}