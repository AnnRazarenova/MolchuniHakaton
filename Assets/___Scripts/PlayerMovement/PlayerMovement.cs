using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, PlayerControls.INewactionmapActions
{
    public static bool InputBlocked { get; set; } = false;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 6f;

    [Header("Interaction")]
    [SerializeField] private float interactRadius = 2.5f;
    [SerializeField] private LayerMask interactableMask = ~0;

    private Interactable nearestInteractable;

    private Rigidbody rb;
    private PlayerControls controls;
    private Vector2 moveInput;
    private bool jumpPressed;
    private bool grounded;

    // ---------- Lifecycle ----------

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        controls = new PlayerControls();
        controls.Newactionmap.SetCallbacks(this);
    }

    void OnEnable() => controls.Newactionmap.Enable();
    void OnDisable() => controls.Newactionmap.Disable();

    // ---------- Input System callbacks ----------

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (InputBlocked) { moveInput = Vector2.zero; return; }
        moveInput = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (InputBlocked) return;
        if (ctx.performed) jumpPressed = true;
    }

    public void OnInterect(InputAction.CallbackContext ctx)
    {
        if (InputBlocked || !ctx.performed) return;
        if (nearestInteractable != null)
            nearestInteractable.Interact();
    }

    // ---------- Per-frame ----------

    void Update()
    {
        if (InputBlocked)
        {
            nearestInteractable = null;
            return;
        }

        UpdateNearestInteractable();
        grounded = Physics.Raycast(transform.position, Vector3.down, 0.15f);
    }

    void FixedUpdate()
    {
        if (InputBlocked)
        {
            // √асим горизонталь, оставл€ем вертикаль (падение)
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            jumpPressed = false;
            return;
        }

        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);
        if (move.sqrMagnitude > 1f) move.Normalize();

        Vector3 vel = move * moveSpeed;
        vel.y = rb.velocity.y;
        if (jumpPressed && grounded) vel.y = jumpForce;

        rb.velocity = vel;
        jumpPressed = false;
    }

    // ---------- Interaction search ----------

    private void UpdateNearestInteractable()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position, interactRadius, interactableMask);

        Interactable best = null;
        float bestDist = float.MaxValue;

        foreach (var hit in hits)
        {
            if (!hit.TryGetComponent(out Interactable interactable)) continue;

            float d = (hit.transform.position - transform.position).sqrMagnitude;
            if (d < bestDist)
            {
                bestDist = d;
                best = interactable;
            }
        }

        nearestInteractable = best;
    }
}
