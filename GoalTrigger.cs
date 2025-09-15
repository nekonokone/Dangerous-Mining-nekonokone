using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (other.CompareTag("Player"))
        {
            triggered = true;
            PlayerController.canMove = false;

            StartCoroutine(GoalSequence(other.gameObject));
        }

    }

    private IEnumerator GoalSequence(GameObject player)
    {
        Vector3 goalCenter = transform.position;
        float moveDuration = 1.5f; // ゆっくり吸い込まれる時間
        float elapsed = 0f;

        Vector3 start = player.transform.position;

        PlayerController.canMove = false;

        while (elapsed < moveDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / moveDuration;

            float smoothT = t * t * (3f - 2f * t);
            player.transform.position = Vector3.Lerp(start, goalCenter, smoothT);

            yield return null;
        }

        // 完全にゴール中心に到達
        player.transform.position = goalCenter;

        // プレイヤー非表示（吸い込まれた）
        player.SetActive(false);

        // リザルトまでの余韻
        yield return new WaitForSecondsRealtime(1f);

        SceneManager.LoadScene("Result2");
    }

}
