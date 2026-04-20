using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 1;

    private void OnTriggerEnter(Collider other)
    {
        TryCollect(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        TryCollect(collision.gameObject);
    }

    private void TryCollect(GameObject other)
    {
        if (!other.CompareTag("Ball")) return;
        if (GameManager.Instance == null) return;

        GameManager.Instance.AddCoins(value);
        Destroy(gameObject);
    }
}
