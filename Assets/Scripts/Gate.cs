using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Gate : MonoBehaviour
{
    public float gateHP = 1000f;

    [Header("UI")]
    public TextMeshProUGUI gateHpText;
    public Image gateHpFill;

    private float _maxHp;

    private void Awake()
    {
        _maxHp = gateHP;
        UpdateGateHpUI();
    }

    private void OnValidate()
    {
        if (gateHP < 0f) gateHP = 0f;
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0f) return;

        gateHP = Mathf.Max(0f, gateHP - damage);
        UpdateGateHpUI();

        if (gateHP <= 0)
        {
            Debug.Log("???? ????????????!");
            GameManager.Instance.EndSession(true); // ???? ???? ????
        }
    }

    private void UpdateGateHpUI()
    {
        if (gateHpText == null) return;

        // ??? ?????? ?????? ??? ?? ???? (????/???/???? ??)
        gateHpText.text = $"HP: {Mathf.CeilToInt(gateHP)}/{Mathf.CeilToInt(_maxHp)}";

        if (gateHpFill != null)
        {
            float denom = _maxHp <= 0f ? 1f : _maxHp;
            gateHpFill.fillAmount = Mathf.Clamp01(gateHP / denom);
        }
    }
}