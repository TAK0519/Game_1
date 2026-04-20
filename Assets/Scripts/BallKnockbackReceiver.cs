using UnityEngine;

/// <summary>
/// 벽·바닥·범퍼 등 <b>움직이지 않는</b> 오브젝트에 붙입니다.
/// <c>Ball</c> 컴포넌트가 있는 공과 충돌할 때, 공의 <see cref="Rigidbody"/>에만 임펄스를 줘서 튕겨 내고
/// 이 오브젝트에는 힘을 가하지 않습니다. (이쪽에는 Rigidbody가 없어도 됩니다. 콜라이더만 있으면 됩니다.)
/// </summary>
public class BallKnockbackReceiver : MonoBehaviour
{
    [SerializeField] float impulse = 10f;
    [SerializeField] float upwardBias = 0f;
    [SerializeField] bool scaleByImpactSpeed = true;
    [SerializeField] float impactSpeedScale = 0.12f;
    [SerializeField] float impactSpeedClampMin = 0.4f;
    [SerializeField] float impactSpeedClampMax = 2.5f;

    void OnCollisionEnter(Collision collision)
    {
        if (!TryGetBall(collision, out Ball ball))
            return;

        Rigidbody ballRb = collision.rigidbody != null
            ? collision.rigidbody
            : ball.GetComponent<Rigidbody>();
        if (ballRb == null || ballRb.isKinematic)
            return;

        if (collision.contactCount == 0)
            return;

        Vector3 n = collision.GetContact(0).normal;
        Vector3 toBall = ballRb.position - collision.GetContact(0).point;
        if (Vector3.Dot(n, toBall) < 0f)
            n = -n;

        if (upwardBias != 0f)
            n = (n + Vector3.up * upwardBias).normalized;
        else
            n = n.normalized;

        if (n.sqrMagnitude < 1e-6f)
            return;

        float mult = 1f;
        if (scaleByImpactSpeed && collision.relativeVelocity.sqrMagnitude > 1e-4f)
        {
            float v = collision.relativeVelocity.magnitude;
            mult = Mathf.Clamp(v * impactSpeedScale, impactSpeedClampMin, impactSpeedClampMax);
        }

        ballRb.AddForce(n * impulse * mult, ForceMode.Impulse);
    }

    static bool TryGetBall(Collision collision, out Ball ball)
    {
        ball = collision.gameObject.GetComponent<Ball>()
            ?? collision.gameObject.GetComponentInParent<Ball>();
        return ball != null;
    }
}
