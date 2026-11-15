using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public enum ThrowState
{
    Held,
    Out,
    Hover,
    Returning
}

public class Shield : MonoBehaviour
{
    public InputActionReference throwAction;
    public Player playerScript;
    public Orbit orbitScript;

    public float inputBuffer = 0.15f;

    public float throwSpeed = 12f;
    public float maxDistance = 6f;
    public float endSlowdown = 0.65f;
    public float hoverTime = 0.35f;

    public float returnStartSpeed = 3f;
    public float returnAcceleration = 50f;
    public float returnMaxSpeed = 18f;
    public float catchDistance = 0.5f;

    public float throwSpin = 1080f;
    public float hoverSpin = 1500f;
    public float returnSpin = 1080f;

    public ThrowState State { get; set; }

    Vector2 direction = Vector2.right;
    float traveled;
    float hoverTimer;
    float returnSpeed;
    float bufferTimer;

    void OnEnable()
    {
        if (throwAction != null)
        {
            throwAction.action.Enable();
            throwAction.action.performed += Throw;
        }
    }

    void OnDisable()
    {
        if (throwAction != null)
        {
            throwAction.action.Disable();
            throwAction.action.performed -= Throw;
        }
    }

    public void Throw(InputAction.CallbackContext context)
    {
        if (State == ThrowState.Held)
        {
            Throw(GetAimDirection());
        }
        else
        {
            bufferTimer = inputBuffer;
        }
    }

    void Update()
    {
        if (bufferTimer > 0f)
        {
            bufferTimer -= Time.deltaTime;

            if (State == ThrowState.Held)
            {
                Throw(GetAimDirection());
            }
        }

        switch (State)
        {
            case ThrowState.Held:
                UpdateHeld();
                break;
            case ThrowState.Out:
                UpdateOut();
                break;
            case ThrowState.Hover:
                UpdateHover();
                break;
            case ThrowState.Returning:
                UpdateReturning();
                break;
        }
    }

    public void Throw(Vector2 throwDirection)
    {
        if (State != ThrowState.Held)
        {
            return;
        }

        if (throwDirection.sqrMagnitude < 0.0001f)
        {
            throwDirection = direction;
        }

        direction = throwDirection.normalized;
        traveled = 0f;
        bufferTimer = 0f;
        State = ThrowState.Out;
    }

    public void Recall()
    {
        if (State == ThrowState.Out || State == ThrowState.Hover)
        {
            returnSpeed = returnStartSpeed;
            State = ThrowState.Returning;
        }
    }

    void UpdateHeld()
    {
        // Nothing for now
    }

    void UpdateOut()
    {
        float t = 0f;

        if (maxDistance > 0f)
        {
            t = Mathf.Clamp01(traveled / maxDistance);
        }

        float slowdown = Mathf.Min(endSlowdown, 0.95f);
        float speed = throwSpeed * (1f - slowdown * t * t);
        float step = Mathf.Min(speed * Time.deltaTime, maxDistance - traveled);

        transform.position += (Vector3)(direction * step);
        traveled += step;
        Spin(throwSpin);

        if (traveled >= maxDistance - 0.0001f)
        {
            hoverTimer = hoverTime;
            State = ThrowState.Hover;
        }
    }

    void UpdateHover()
    {
        hoverTimer -= Time.deltaTime;
        Spin(hoverSpin);

        if (hoverTimer <= 0f)
        {
            returnSpeed = returnStartSpeed;
            State = ThrowState.Returning;
        }
    }

    void UpdateReturning()
    {
        if (playerScript.gameObject == null)
        {
            Catch();
            return;
        }

        returnSpeed = Mathf.Min(returnSpeed + returnAcceleration * Time.deltaTime, returnMaxSpeed);

        Vector2 toOwner = (Vector2)orbitScript.position - (Vector2)transform.position;
        float distance = toOwner.magnitude;

        if (distance <= catchDistance)
        {
            Catch();
            return;
        }

        float step = Mathf.Min(returnSpeed * Time.deltaTime, distance);
        transform.position += (Vector3)(toOwner.normalized * step);
        Spin(returnSpin);
    }

    void Catch()
    {
        State = ThrowState.Held;
    }

    Vector2 GetAimDirection()
    {
        return playerScript.normalizedDir;
    }

    void Spin(float speed)
    {
        transform.Rotate(0f, 0f, speed * Time.deltaTime);
    }
}