using UnityEngine;

public class Gate : MonoBehaviour
{
    public float gateHP = 1000f;

    public void TakeDamage(float damage)
    {
        gateHP -= damage;
        Debug.Log($"Gate HP: {gateHP}");

        if (gateHP <= 0)
        {
            Debug.Log("문이 파괴되었습니다!");
            GameManager.Instance.EndSession(true); // 승리 세션 종료
        }
    }
}