using System.Collections;
using UnityEngine;
using TMPro;

public class GameStartManager : MonoBehaviour
{
    public TextMeshProUGUI countdownText;

    void Start()
    {
        SoundManager.Instance.StopBgm();
        GameResultManager.Reset();
        SoundManager.Instance.PlayBgm(BGMType.Stage);
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        PlayerMove.inputEnabled = false;
        Time.timeScale = 0f;

        countdownText.gameObject.SetActive(true);

        yield return CountdownStep("3");
        yield return CountdownStep("2");
        yield return CountdownStep("1");
        yield return CountdownStep("START!");

        countdownText.gameObject.SetActive(false);
        Time.timeScale = 1f;
        PlayerMove.inputEnabled = true;
    }

    IEnumerator CountdownStep(string text)
    {
        countdownText.text = text;
        float timer = 0f;

        while (timer < 1f)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
    }
}