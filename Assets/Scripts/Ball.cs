using UnityEngine;

public class Ball : MonoBehaviour
{
    public float currentPower = 0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;

        // ���� ���� �ӵ��� �Ŀ� ��꿡 �ݿ� (�������� ����ġ ���)
        float speedMultiplier = Mathf.Clamp(rb.linearVelocity.magnitude * 0.1f, 1f, 3f);

        switch (tag)
        {
            case "Wall": // �Ϲ� ��: �⺻ �Ŀ�
                AddPower(10f * speedMultiplier);
                break;

            case "Bumper": // Ư�� ����: ���� �Ŀ� + �ݹ߷�
                AddPower(50f * speedMultiplier);
                // ���� ����� �ݹ߷� �߰� (���� ����)
                rb.AddForce(collision.contacts[0].normal * -500f);
                break;

            case "Gate": // ��: ����
                ApplyDamageToGate(collision.gameObject);
                break;
        }
    }

    void AddPower(float amount)
    {
        currentPower += amount;
        GameManager.Instance.UpdatePowerUI(currentPower);
        // �Ŀ��� ���� ������ ���� ũ�⳪ ������ ��¦ ���ϰ� �ϸ� �ǵ���� �� Ȯ���մϴ�.
    }

    void ApplyDamageToGate(GameObject gateObj)
    {
        Gate gate = gateObj.GetComponent<Gate>();
        if (gate != null && currentPower > 0)
        {
            gate.TakeDamage(currentPower);
            currentPower = 0;
            GameManager.Instance.UpdatePowerUI(currentPower);
        }
    }
}