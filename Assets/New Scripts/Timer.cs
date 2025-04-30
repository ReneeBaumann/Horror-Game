using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 180f; // 3 minutes
    public TextMeshProUGUI timerText;
    public string gameOverSceneName = "GameOverScene";
    public Color warningColor = Color.red;
    public float warningThreshold = 10f; // Flash when 10 seconds left

    private bool isWarningFlashing = false;

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay();

            if (timeRemaining <= warningThreshold && !isWarningFlashing)
            {
                isWarningFlashing = true;
                StartCoroutine(FlashWarning());
            }
        }
        else
        {
            SceneManager.LoadScene(gameOverSceneName);
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    System.Collections.IEnumerator FlashWarning()
    {
        while (timeRemaining > 0)
        {
            timerText.color = warningColor;
            yield return new WaitForSeconds(0.5f);
            timerText.color = Color.white;
            yield return new WaitForSeconds(0.5f);
        }
    }
}