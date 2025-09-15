using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public float timeLimit = 180f;
    public static float elapsedTime = 0f;
    private bool isFinished = false;

    void Start()
    {
        elapsedTime = 0f;
    }
    public TextMeshProUGUI timerText;

    void Update()
    {
        if (isFinished) return;

        elapsedTime += Time.deltaTime;

        if (timerText != null)
        {
            float remainingTime = Mathf.Max(0, timeLimit - elapsedTime);
            timerText.text = Mathf.FloorToInt(remainingTime).ToString();
        }

        if (elapsedTime >= timeLimit)
        {
            isFinished = true;
            GameResultManager.timeOver = true;
            SceneManager.LoadScene("Result2"); //あとで書き換え
        }
    }

    public static float GetElapsedTime()
    {
        return elapsedTime;
    }

    public void FinishGame()
    {
        if (!isFinished)
        {
            isFinished = true;
            GameResultManager.timeOver = false;
            SceneManager.LoadScene("Result2");　//あとで書き換え
        }
    }
}
