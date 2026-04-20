using UnityEngine;

public class Ball : MonoBehaviour
{
    public float currentPower = 0f;

    [Header("Lives")]
    public int maxLives = 5;
    [SerializeField] private int currentLives;

    private Rigidbody rb;

    public int CurrentLives => currentLives;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (currentLives <= 0) currentLives = maxLives;
        if (GameManager.Instance != null) GameManager.Instance.UpdateLivesUI(this);
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

    public void TakeHit(int amount = 1)
    {
        if (amount <= 0) return;

        currentLives = Mathf.Max(0, currentLives - amount);
        if (GameManager.Instance != null) GameManager.Instance.UpdateLivesUI(this);

        if (currentLives <= 0)
        {
            Destroy(gameObject);
        }
    }

    public bool TryAddLives(int amount = 1)
    {
        if (amount <= 0) return false;
        if (currentLives >= maxLives) return false;

        currentLives = Mathf.Min(maxLives, currentLives + amount);
        if (GameManager.Instance != null) GameManager.Instance.UpdateLivesUI(this);
        return true;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null) GameManager.Instance.UpdateLivesUI(null);
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