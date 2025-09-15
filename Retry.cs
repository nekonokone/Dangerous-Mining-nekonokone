using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retry : MonoBehaviour
{
    public void OnContinueButtonClicked()
    {
        // 直前のシーンに遷移する。
        SceneController.BackToBeforeScene();
    }
}