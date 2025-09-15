using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("テキスト")]
    public TextMeshProUGUI normText;
    public TextMeshProUGUI lUseText;
    public TextMeshProUGUI coneUseText;

    [Header("スタミナUI")]
    public Slider staminaSlider;

    [Header("スキル クールダウンUI")]
    public Image skillLFill;
    public float cooldownL = 3f;
    public Image skillKFill;
    public float cooldownK = 2f;

    [Header("ノルマ設定")]
    public int normTarget = 6;

    private float tL = 0f;
    private float tK = 0f;

    public enum Skill { L, K }

    void Start()
    {
        if (staminaSlider != null) staminaSlider.gameObject.SetActive(false);
        if (skillLFill != null) skillLFill.fillAmount = 0f;
        if (skillKFill != null) skillKFill.fillAmount = 0f;
    }

    void Update()
    {
        if (tL > 0f)
        {
            tL -= Time.deltaTime;
            if (tL < 0f) tL = 0f;

            if (skillLFill != null)
                skillLFill.fillAmount = Mathf.Clamp01(tL / cooldownL);
        }
        if (tK > 0f)
        {
            tK -= Time.deltaTime;
            if (tK < 0f) tK = 0f;

            if (skillKFill != null)
                skillKFill.fillAmount = Mathf.Clamp01(tK / cooldownK);
        }
    }

    public void UpdateUI(int collectCount, int lUsesLeft, int maxLUses, int coneUsesLeft, int maxConeUses)
    {
        if (normText != null) normText.text = $"Ore {collectCount}/{normTarget}";
        if (lUseText != null) lUseText.text = $"{lUsesLeft}/{maxLUses}";
        if (coneUseText != null) coneUseText.text = $"{coneUsesLeft}/{maxConeUses}";
    }

    public void UpdateStamina(float current, float max)
    {
        if (staminaSlider == null) return;
        staminaSlider.maxValue = max;
        staminaSlider.value = current;
    }
    public void ShowStaminaUI(bool show)
    {
        if (staminaSlider != null) staminaSlider.gameObject.SetActive(show);
    }

    public bool IsSkillReady(Skill s)
    {
        return (s == Skill.L) ? (tL <= 0f) : (tK <= 0f);
    }

    public void TriggerSkill(Skill s)
    {
        if (s == Skill.L)
        {
            if (tL > 0f) return;
            tL = cooldownL;
            if (skillLFill != null) skillLFill.fillAmount = 1f;
        }
        else
        {
            if (tK > 0f) return;
            tK = cooldownK;
            if (skillKFill != null) skillKFill.fillAmount = 1f;
        }
    }
}
