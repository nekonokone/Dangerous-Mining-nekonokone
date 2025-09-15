using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtonHandler : MonoBehaviour
{
    public string sceneToLoad = "StageSelect";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }

    public void OnStartGamePressed()
    {
        StartGame();
    }

    private void StartGame()
    {
        PlayerController.canMove = true;

        SceneManager.LoadScene(sceneToLoad);
    }
}
