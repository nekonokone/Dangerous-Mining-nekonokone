using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retry : MonoBehaviour
{
    public void OnContinueButtonClicked()
    {
        // ���O�̃V�[���ɑJ�ڂ���B
        SceneController.BackToBeforeScene();
    }
}