using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeTransitionManager : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    private void Start()
    {
        SoundManager.Instance.PlayBgm(BGMType.Title);
    }
    public void StartFade(string sceneName)
    {
        StartCoroutine(FadeAndSwitch(sceneName));
    }

    IEnumerator FadeAndSwitch(string sceneName)
    {
        float timer = 0f;
        Color c = fadeImage.color;
        fadeImage.gameObject.SetActive(true);

        while (timer < fadeDuration)
        {
            float t = timer / fadeDuration;
            fadeImage.color = new Color(c.r, c.g, c.b, Mathf.Lerp(0, 1, t));
            timer += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = new Color(c.r, c.g, c.b, 1);
        SceneManager.LoadScene(sceneName);
        SoundManager.Instance.StopBgm();
    }
}