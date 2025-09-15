using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    [Header("移動・ジャンプ")]
    public float moveSpeed = 0.05f;
    public float dashMultiplier = 2f;
    public float jumpForce = 10f;
    public int maxJumpCount = 2;

    [Header("採掘・スキル")]
    public LayerMask oreLayer;
    public GameObject coneTriggerObject;
    public GameObject breakEffectPrefab;

    [Header("スタミナ設定")]
    public float maxStamina = 100f;
    public float staminaDrainPerSecond = 20f;
    public float staminaRegenDelay = 2f;
    public float staminaRegenPerSecond = 15f;

    public UIManager uiManager;

    private float currentStamina;
    private float regenCooldown = 0f;
    private bool isDashing = false;

    public int maxLUses = 5;
    public int maxConeUses = 3;

    private int currentJumpCount = 0;
    private int collectCount = 0;
    private int lUsesLeft;
    private int coneUsesLeft;

    private Animator animator;
    private OreAnimator currentOre;
    private bool isCollecting = false;
    public Ground_Check_C Ground_Check_D;

    private ability abilityScript;
    private Rigidbody2D rb;

    public static bool inputEnabled = true;

    [SerializeField] float flashInterval;
    [SerializeField] int loopCount;
    SpriteRenderer sp;
    bool isHit;

    void Start()
    {
        //SpriteRenderer格納
        sp = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        abilityScript = GetComponent<ability>();

        lUsesLeft = maxLUses;
        coneUsesLeft = maxConeUses;
        currentStamina = maxStamina;

        if (uiManager != null)
        {
            uiManager.UpdateStamina(currentStamina, maxStamina);
            uiManager.ShowStaminaUI(false);
            uiManager.UpdateUI(collectCount, lUsesLeft, maxLUses, coneUsesLeft, maxConeUses);
        }
    }

    void Update()
    {
        if (rb != null) rb.velocity = new Vector2(0f, rb.velocity.y);
        if (!inputEnabled) return;
        float baseSpeed = moveSpeed;
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        isDashing = shiftHeld && currentStamina > 0f;

        float step = isDashing ? baseSpeed * dashMultiplier : baseSpeed;

        if (Input.GetKey(KeyCode.D))
        {
            animator.SetBool("aruki", true);
            Vector2 pos = transform.position;
            pos.x += step;
            transform.position = pos;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            animator.SetBool("aruki", true);
            Vector2 pos = transform.position;
            pos.x -= step;
            transform.position = pos;
        }
        else
        {
            animator.SetBool("aruki", false);
        }

        float moveInput = Input.GetAxisRaw("Horizontal");
        Vector2 lscale = transform.localScale;
        if ((lscale.x > 0 && moveInput < 0) || (lscale.x < 0 && moveInput > 0))
        {
            lscale.x *= -1;
            transform.localScale = lscale;
        }

        if (isDashing)
        {
            currentStamina -= staminaDrainPerSecond * Time.deltaTime;
            if (currentStamina < 0f) currentStamina = 0f;
            regenCooldown = staminaRegenDelay;

            if (uiManager != null)
            {
                uiManager.ShowStaminaUI(true);
                uiManager.UpdateStamina(currentStamina, maxStamina);
            }
        }
        else
        {
            if (regenCooldown > 0f) regenCooldown -= Time.deltaTime;
            else if (currentStamina < maxStamina)
                currentStamina += staminaRegenPerSecond * Time.deltaTime;

            if (uiManager != null)
            {
                bool show = currentStamina < maxStamina;
                uiManager.ShowStaminaUI(show);
                uiManager.UpdateStamina(currentStamina, maxStamina);
            }
        }

        if (Ground_Check_D != null && Ground_Check_D.is_Ground)
        {
            animator.SetBool("jump", false);
            currentJumpCount = 0;
        }
        //

        if (Input.GetKeyDown(KeyCode.Space) && currentJumpCount < maxJumpCount)
        {
            animator.SetBool("jump", true);
            SoundManager.Instance.PlaySe(SEType.Jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            currentJumpCount++;
        }


        if (Input.GetKey(KeyCode.S))
        {
            animator.SetBool("mine", true);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2f, oreLayer);
            if (hit.collider != null && hit.collider.CompareTag("ore"))
            {
                OreAnimator newOre = hit.collider.GetComponent<OreAnimator>();
                if (newOre != null)
                {

                    if (currentOre != newOre)
                    {
                        if (currentOre != null) currentOre.StopMining();
                        currentOre = newOre;
                        currentOre.StartMining();
                    }

                    if (currentOre.UpdateMining(Time.deltaTime))
                    {

                        DestroyOreBelow(false);
                        collectCount++;
                        UpdateUI();
                        currentOre = null;
                    }
                }
            }
        }
        else
        {
            animator.SetBool("mine", false);
            if (currentOre != null)
            {
                currentOre.StopMining();
                currentOre = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.L) && lUsesLeft > 0 && uiManager != null && uiManager.IsSkillReady(UIManager.Skill.L))
        {
            if (CheckOreBelow())
            {
                collectCount++;
                lUsesLeft--;
                DestroyOreBelow(true);

                uiManager.TriggerSkill(UIManager.Skill.L);
                UpdateUI();
            }
        }

        if (Input.GetKeyDown(KeyCode.K) && coneUsesLeft > 0 && uiManager != null && uiManager.IsSkillReady(UIManager.Skill.K))
        {
            var trig = coneTriggerObject != null ? coneTriggerObject.GetComponent<ConeTrigger>() : null;
            if (trig == null)
            {
                Debug.LogWarning("[Player] coneTriggerObject が未設定");
            }
            else
            {
                if (trig.TriggerObstaclesOnce())
                {
                    coneUsesLeft--;
                    uiManager.TriggerSkill(UIManager.Skill.K);
                    uiManager.UpdateUI(collectCount, lUsesLeft, maxLUses, coneUsesLeft, maxConeUses: 3);
                }
            }
        }
    }
    IEnumerator CheckCollect()
    {
        isCollecting = true;
        yield return new WaitForSeconds(2f);
        if (CheckOreBelow())
        {
            collectCount++;
            DestroyOreBelow(false);
            UpdateUI();
        }
        isCollecting = false;
    }
    bool CheckOreBelow()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2f, oreLayer);
        if (hit.collider != null && hit.collider.CompareTag("ore"))
        {
            OreAnimator anim = hit.collider.GetComponent<OreAnimator>();
            if (anim != null) anim.StartMining();
            return true;
        }
        return false;
    }

    void DestroyOreBelow(bool withEffect = true)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2f, oreLayer);
        if (hit.collider != null && hit.collider.CompareTag("ore"))
        {
            if (withEffect && breakEffectPrefab != null)
            {
                GameObject effect = Instantiate(breakEffectPrefab, hit.collider.transform.position, Quaternion.identity);
                Destroy(effect, 2f);
            }
            Destroy(hit.collider.gameObject);
            SoundManager.Instance.StopLoopSe();
        }
    }

     public void DecreaseOre(int amount = 1)
    {
        collectCount = Mathf.Max(0, collectCount - amount);
        UpdateUI();
    }

    void UpdateUI()
    {
        if (uiManager != null)
        {
            uiManager.UpdateUI(collectCount, lUsesLeft, maxLUses, coneUsesLeft, maxConeUses);
        }
        GameResultManager.collectedNorms = collectCount;
        GameResultManager.normTarget = 6;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Rock"))
        {
            animator.SetBool("damage", true);
            SceneController.CurrentSceneName();
            GameResultManager.isDead = true;
            Invoke("GameOver", 5f);
            SoundManager.Instance.StopBgm();
        }

        if (collision.gameObject.CompareTag("FallRock"))
        {
            StartCoroutine(_hit());
        }
    }
    void GameOver()
    {

        SceneManager.LoadScene("Result2");
    }

    //点滅させる処理
    IEnumerator _hit()
    {
        //当たりフラグをtrueに変更（当たっている状態）
        isHit = true;

        //点滅ループ開始
        for (int i = 0; i < loopCount; i++)
        {
            //flashInterval待ってから
            yield return new WaitForSeconds(flashInterval);
            //spriteRendererをオフ
            sp.enabled = false;

            //flashInterval待ってから
            yield return new WaitForSeconds(flashInterval);
            //spriteRendererをオン
            sp.enabled = true;
        }

        //点滅ループが抜けたら当たりフラグをfalse(当たってない状態)
        isHit = false;
    }
}
