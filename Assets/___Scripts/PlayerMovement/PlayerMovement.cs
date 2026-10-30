using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, PlayerControls.INewactionmapActions
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 6f;

    private Rigidbody rb;
    private PlayerControls controls;
    private Vector2 moveInput;
    private bool jumpPressed;
    private bool grounded;

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

    public void OnMove(InputAction.CallbackContext ctx) => moveInput = ctx.ReadValue<Vector2>();
    public void OnJump(InputAction.CallbackContext ctx) { if (ctx.performed) jumpPressed = true; }
    public void OnInterect(InputAction.CallbackContext ctx) { if (ctx.performed) Debug.Log("Interact"); }

    void FixedUpdate()
    {
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);
        if (move.sqrMagnitude > 1f) move.Normalize();

        Vector3 vel = move * moveSpeed;
        vel.y = rb.velocity.y;
        if (jumpPressed && grounded) vel.y = jumpForce;

        rb.velocity = vel;
        jumpPressed = false;
    }

    void OnCollisionStay(Collision c)
    {
        foreach (var ct in c.contacts)
            if (ct.normal.y > 0.5f) { grounded = true; return; }
    }
    void OnCollisionExit(Collision c) => grounded = false;
}
