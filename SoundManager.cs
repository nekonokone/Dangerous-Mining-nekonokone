using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGMType
{
    Title,
    Stage,
    Clear,
    Failed,
    Null
}

[System.Serializable]
struct BGMData
{
    public BGMType Type;
    public AudioClip Clip;
    [Range(0, 1)]
    public float Volume;
    public bool Loop;
}

public enum SEType
{
    Jump,
    Collect
}
[System.Serializable]
struct SEData
{
    public SEType Type;
    public AudioClip Clip;
    [Range(0, 1)]
    public float Volume;
    public bool Loop;
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get => instance; }
    //�Q�[�����ōĐ�����BGM�̃��X�g
    [SerializeField]
    private List<BGMData> bgmDataList = new List<BGMData>();

    [SerializeField]
    private List<SEData> seDataList = new List<SEData>();

    [SerializeField]
    private AudioSource bgmSource = null;

    [SerializeField]
    private AudioSource seSource = null;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        { instance = this; }
        else return;
        //PlayBgm(BGMType.Title);
        DontDestroyOnLoad(this.gameObject);

    }
    //BGM�Đ�
    public void PlayBgm(BGMType type)
    {
        if (type == BGMType.Null) return;
        var bgm = bgmDataList[(int)type];
        bgmSource.clip = bgm.Clip;
        bgmSource.volume = bgm.Volume;
        bgmSource.loop = bgm.Loop;
        bgmSource.Play();
    }
    public void StopBgm()
    {
        bgmSource.Stop();
    }

    public void PlaySe(SEType type)
    {
        var se = seDataList[(int)type];
        seSource.clip = se.Clip;
        seSource.volume = se.Volume;
        seSource.PlayOneShot(se.Clip);
    }
    //�T�E���h���[�v�Đ�
    public void PlayLoopSe(SEType type)
    {
        var se = seDataList[(int)type];
        seSource.clip = se.Clip;
        seSource.loop = se.Loop;
        seSource.volume = se.Volume;
        seSource.Play();
    }

    public void StopLoopSe()
    {
        seSource.Stop();
    }

    // �s���ǋL
    public void StopLoopBgm()
    {
        seSource.Stop();
    }
}
