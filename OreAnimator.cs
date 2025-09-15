using System.Collections;
using UnityEngine;

public class OreAnimator : MonoBehaviour
{
    public Sprite[] oreSprites;
    public float frameDuration = 0.5f;
    public float requiredTimeToBreak = 2f; // 掘るのにかかる時間

    private SpriteRenderer spriteRenderer;
    private int currentIndex = 0;
    private Coroutine animationCoroutine;
    private Sprite originalSprite;
    private float miningTimer = 0f;

    public bool IsMining { get; private set; }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
    }

    public void StartMining()
    {
        SoundManager.Instance.PlayLoopSe(SEType.Collect);
        if (!IsMining)
        {
            IsMining = true;
            miningTimer = 0f;
            animationCoroutine = StartCoroutine(PlayAnimation());
        }
    }

    public void StopMining()
    {
        Debug.Log("はしてない");
        
        if (IsMining)
        {
            IsMining = false;

            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
                animationCoroutine = null;
            }

            miningTimer = 0f;
            currentIndex = 0;
            spriteRenderer.sprite = originalSprite;
        }
    }


    public bool UpdateMining(float deltaTime)
    {
        if (!IsMining) return false;

        miningTimer += deltaTime;

        if (miningTimer >= requiredTimeToBreak)
        {
            IsMining = false;
            return true;
        }

        return false;
    }

    IEnumerator PlayAnimation()
    {
        while (true)
        {
            spriteRenderer.sprite = oreSprites[currentIndex];
            currentIndex = (currentIndex + 1) % oreSprites.Length;
            yield return new WaitForSeconds(frameDuration);
        }
    }
}
