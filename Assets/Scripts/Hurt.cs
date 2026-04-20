using UnityEngine;

public class Hurt : MonoBehaviour
{
    public int damageLives = 1;
    public float hitCooldownSeconds = 0.2f;

    private float _nextHitTime;

    private void OnCollisionEnter(Collision collision)
    {
        TryDamageBall(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        TryDamageBall(other.gameObject);
    }

    private void TryDamageBall(GameObject other)
    {
        if (hitCooldownSeconds > 0f && Time.time < _nextHitTime) return;
        if (!other.CompareTag("Ball")) return;

        Ball ball = other.GetComponent<Ball>();
        if (ball == null) return;

        ball.TakeHit(damageLives);

        if (hitCooldownSeconds > 0f)
        {
            _nextHitTime = Time.time + hitCooldownSeconds;
        }
    }
}