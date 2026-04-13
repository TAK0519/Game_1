using UnityEngine;
using UnityEngine.InputSystem;

public class FlipperScript : MonoBehaviour
{
    public HingeJoint hinge;
    public float hitStrength = 50000f;
    public float damper = 1000f;

    // New Input System의 Action을 연결
    public InputAction flipperAction;

    void OnEnable() => flipperAction.Enable();
    void OnDisable() => flipperAction.Disable();

    void Update()
    {
        JointSpring spring = hinge.spring;
        spring.spring = hitStrength;
        spring.damper = damper;

        // 버튼을 누르고 있으면(Action 수행 중이면) 45도, 아니면 -45도
        float isPressed = flipperAction.ReadValue<float>();
        spring.targetPosition = (isPressed > 0.5f) ? 60f : -60f;

        hinge.spring = spring;
        hinge.useSpring = true;
    }
}