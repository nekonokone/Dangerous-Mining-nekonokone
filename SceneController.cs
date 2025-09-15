using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// �i�|�C���g�j
// static�N���X�i�ÓI�N���X�j�ɕύX���邱��
// ���\�b�h��S��static�i�ÓI���\�b�h�j�ɂ��邱��
// MonoBehaviour�N���X�͍폜���邱��
public static class SceneController
{
    public static string sceneName;

    // �u���̃��\�b�h�����s���ꂽ���ɊJ���Ă���V�[���̖��O�v��擾����B
    // ����̏ꍇ�́A�Q�[���I�[�o�[�̏�������������ɁA���̃��\�b�h��Ăяo���B
    public static void CurrentSceneName()
    {
        sceneName = SceneManager.GetActiveScene().name;
        Debug.Log(sceneName);
    }

    // ��L�̃��\�b�h�Ŏ擾���ꂽ�V�[���ɖ߂�B
    // ����̏ꍇ�́A�R���e�B�j���[�{�^������������ɂ��̃��\�b�h����s����B
    public static void BackToBeforeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}