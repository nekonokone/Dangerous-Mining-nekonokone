using UnityEngine;
using System.Collections;

public class SignJitter : MonoBehaviour
{
    public float intervalMin = 3f;
    public float intervalMax = 6f;
    public float shakeAmount = 10f; // 揺れ角度（±度）
    public int shakeCount = 5; // 揺れる回数
    public float shakeSpeed = 0.05f; // 揺れの間隔（秒）

    private Quaternion originalRotation;

    void Start()
    {
        originalRotation = transform.localRotation;
        StartCoroutine(JitterLoop());
    }

    IEnumerator JitterLoop()
    {
        while (true)
        {
            // ランダムな時間待つ
            float wait = Random.Range(intervalMin, intervalMax);
            yield return new WaitForSeconds(wait);

            for (int i = 0; i < shakeCount; i++)
            {
                float angle = Random.Range(-shakeAmount, shakeAmount);
                transform.localRotation = Quaternion.Euler(0, 0, angle);
                yield return new WaitForSeconds(shakeSpeed);
            }

            // 元の角度に戻す
            transform.localRotation = originalRotation;
        }
    }
}
