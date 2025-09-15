using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class ResultDisplay : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI normCountText;
    public TextMeshProUGUI rankText;

    public GameObject clearImage;
    public GameObject clearImage2;
    public GameObject failedImage;
    public GameObject failedImage2;

    public GameObject SrankImage;
    public GameObject ArankImage;
    public GameObject BrankImage;
    public GameObject FrankImage;

    public Button titleButton;
    //public Button retryButton;

    void Start()
    {
        float time = GameTimer.GetElapsedTime();
        timeText.text = $"Time: {Mathf.FloorToInt(time)}";
        normCountText.text = $"Ore: {GameResultManager.collectedNorms}/{GameResultManager.normTarget}";

        var rank = GameResultManager.GetRank();

        switch (rank)
        {
            case GameResultManager.ResultRank.F:
                resultText.text = "Failed";
                rankText.text = "Rank: F";
                resultText.color = Color.red;
                break;

            case GameResultManager.ResultRank.B:
                resultText.text = "Clear";
                rankText.text = "Rank: B";
                resultText.color = Color.yellow;
                break;

            case GameResultManager.ResultRank.A:
                resultText.text = "Clear";
                rankText.text = "Rank: A";
                resultText.color = Color.green;
                break;

            case GameResultManager.ResultRank.S:
                resultText.text = "Clear";
                rankText.text = "Rank: S";
                resultText.color = Color.cyan;
                break;
        }

        SrankImage.SetActive(rank == GameResultManager.ResultRank.S);
        ArankImage.SetActive(rank == GameResultManager.ResultRank.A);
        BrankImage.SetActive(rank == GameResultManager.ResultRank.B);
        FrankImage.SetActive(rank == GameResultManager.ResultRank.F);


        clearImage2.SetActive(rank == GameResultManager.ResultRank.S);
        bool isClear = (rank != GameResultManager.ResultRank.F);
        clearImage.SetActive(isClear);
        failedImage.SetActive(!isClear);
        failedImage2.SetActive(!isClear);

        titleButton.onClick.AddListener(() => SceneManager.LoadScene("Title2"));
        //retryButton.onClick.AddListener(() =>
        //{
        //  SceneController.BackToBeforeScene();
        //});

        // 結果SE
        PlayResultSE();
    }

    private void PlayResultSE()
    {
        if (SoundManager.Instance == null) return;

        if (GameResultManager.IsClear())
            SoundManager.Instance.PlayBgm(BGMType.Clear);
        else
            SoundManager.Instance.PlayBgm(BGMType.Failed);
    }
}