using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConeTrigger : MonoBehaviour
{
    [Header("見た目")]
    public GameObject coneMarkPrefab;
    public float stopDuration = 3f;
    public float blinkStartTime = 2f;
    public float blinkInterval = 0.2f;

    [Header("対象タグ（複数可）")]
    public string[] targetTags = { "Rock", "SpawnedObject" };

    private bool isRunning = false;
    public bool TriggerObstaclesOnce()
    {
        if (isRunning)
        {
            Debug.Log("[ConeTrigger] ");
            return false;
        }
        if (coneMarkPrefab == null)
        {
            Debug.LogWarning("[ConeTrigger] coneMarkPrefab が未設定");
            return false;
        }

        StartCoroutine(HandleAllTargets());
        return true;
    }

    private IEnumerator HandleAllTargets()
    {
        isRunning = true;

        HashSet<GameObject> allTargets = new HashSet<GameObject>();
        foreach (string tag in targetTags)
        {
            GameObject[] taggedObjects = null;
            try
            {
                taggedObjects = GameObject.FindGameObjectsWithTag(tag);
            }
            catch
            {
                Debug.LogWarning($"[ConeTrigger] タグ '{tag}'未登録");
                continue;
            }

            foreach (GameObject obj in taggedObjects)
            {
                allTargets.Add(obj);
            }
        }

        if (allTargets.Count == 0)
        {
            Debug.Log("[ConeTrigger] 対象ゼロ。");
            isRunning = false;
            yield break;
        }

        Debug.Log($"[ConeTrigger] 対象数: {allTargets.Count}");

        List<IEnumerator> coros = new List<IEnumerator>();
        foreach (var t in allTargets)
            coros.Add(HandleObstacle(t));

        foreach (var c in coros) StartCoroutine(c);

        float elapsed = 0f;
        while (elapsed < stopDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        isRunning = false;
    }

    private IEnumerator HandleObstacle(GameObject obstacle)
    {
        if (obstacle == null) yield break;

        var rb = obstacle.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogWarning($"[ConeTrigger] {obstacle.name} rbなし");
            yield break;
        }

        var originalBodyType = rb.bodyType;
        var originalVelocity = rb.velocity;

        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        GameObject mark = Instantiate(coneMarkPrefab, obstacle.transform.position, Quaternion.identity);
        var follower = mark.AddComponent<MarkFollower>();
        follower.SetTarget(obstacle.transform);

        var renderer = mark.GetComponentInChildren<SpriteRenderer>();
        float timer = 0f;
        bool blinking = false;

        while (timer < stopDuration && obstacle != null)
        {
            if (!blinking && timer >= blinkStartTime)
            {
                blinking = true;
                if (renderer != null) StartCoroutine(BlinkMark(renderer));
            }

            if (mark != null && obstacle != null)
            {
                var p = obstacle.transform.position;
                p.z = mark.transform.position.z;
                mark.transform.position = p;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        if (mark != null) Destroy(mark);
        if (rb != null && obstacle != null)
        {
            rb.bodyType = originalBodyType;
            rb.velocity = originalVelocity;
        }
    }

    private IEnumerator BlinkMark(SpriteRenderer renderer)
    {
        while (renderer != null)
        {
            renderer.enabled = !renderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
