using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour
{
    public string nextSceneName; // Name of the next scene to load
    public float delay = 0.5f; // Delay in seconds before loading the next scene
    public GameObject fadeout;

    private bool playerInsideTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger.");
            playerInsideTrigger = true;

            if (fadeout != null)
            {
                fadeout.SetActive(true);
                Debug.Log("Fadeout activated.");
            }
            else
            {
                Debug.LogWarning("Fadeout GameObject is not assigned!");
            }

            Invoke("LoadNextScene", delay);
        }
    }

    private void LoadNextScene()
    {
        if (playerInsideTrigger)
        {
            Debug.Log("Loading scene: " + nextSceneName);
            SceneManager.LoadScene(nextSceneName);
        }
    }
}