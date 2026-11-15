using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public InputActionReference moveAction;
    public InputActionReference dashAction;
    public InputActionReference throwAction;

    public Camera mainCam;

    [Header("Normal Movement Stuff")]
    public float maxSpeed = 5f;
    public float acceleration = 40f;
    public float deceleration = 60f;
    public float speedMultiplier = 1f;

    [Header("Dash Stuff")]
    public float dashDuration = 0.5f;
    public float dashSpeed = 40f;
    public float dashCooldown = 2f;

    [Header("Throw Shield Stuff")]
    // Things will go here

    public bool canMove = true;
    bool isDashing = false;

    Rigidbody2D rb;
    Vector2 input;
    public Vector2 normalizedDir;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void OnEnable()
    {
        if (moveAction != null)
        {
            moveAction.action.Enable();
        }

        if (dashAction != null)
        {
            dashAction.action.Enable();
            dashAction.action.performed += DashCall;
        }
    }

    void OnDisable()
    {
        if (moveAction != null)
        {
            moveAction.action.Disable();
        }

        if (dashAction != null)
        {
            dashAction.action.Disable();
            dashAction.action.performed -= DashCall;
        }

        input = Vector2.zero;
    }

    void Update()
    {
        input = Vector2.zero;

        if (canMove && moveAction != null)
        {
            input = Vector2.ClampMagnitude(moveAction.action.ReadValue<Vector2>(), 1f);
        }

        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

        Vector3 mouseWorldPosition = mainCam.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, mainCam.nearClipPlane));

        mouseWorldPosition.z = 0;

        Vector2 direction = mouseWorldPosition - transform.position;
        normalizedDir = direction.normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void FixedUpdate()
    {
        Vector2 target = maxSpeed * speedMultiplier * input;
        float rate = deceleration;

        if (input.sqrMagnitude > 0.0001f)
        {
            rate = acceleration;
        }

        rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, target, rate * Time.fixedDeltaTime);
    }

    public void DashCall(InputAction.CallbackContext context)
    {
        if (!isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    public IEnumerator Dash()
    {
        isDashing = true;
        rb.linearVelocity = normalizedDir * dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(dashCooldown);
        isDashing = false;
    }

}
