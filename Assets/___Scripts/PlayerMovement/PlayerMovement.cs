using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, PlayerControls.INewactionmapActions
{
    public static bool InputBlocked { get; set; } = false;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 6f;

    [Header("References")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;

    [Header("Idle Sprites")]
    [SerializeField] private Sprite _idleForward;
    [SerializeField] private Sprite _idleBackward;
    [SerializeField] private Sprite _idleSide;

    [Header("Interaction")]
    [SerializeField] private float interactRadius = 1f;
    [SerializeField] private LayerMask interactableMask = ~0;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource _stepAudioSource;

    private Interactable nearestInteractable;
    private Rigidbody rb;
    private PlayerControls controls;
    private Vector2 moveInput;
    private bool jumpPressed;
    private bool grounded;

    // Текущее направление взгляда (для Idle)
    private Vector2 lastDir = Vector2.down;

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

    void Update()
    {
        if (InputBlocked)
        {
            nearestInteractable = null;
            return;
        }

        UpdateNearestInteractable();
        grounded = Physics.Raycast(transform.position, Vector3.down, 1.15f, ~LayerMask.GetMask("UI", "Ignore Raycast"));

        bool isMoving = moveInput.sqrMagnitude > 0.1f;

        if (isMoving && grounded) {
            if(!_stepAudioSource.isPlaying)
                _stepAudioSource.Play();
        } else {
            _stepAudioSource.Stop();
        }

        if (isMoving)
        {
            lastDir = moveInput.normalized;

            // Включаем Animator, чтобы он проигрывал ходьбу
            _animator.enabled = true;

            _animator.SetFloat("MoveX", moveInput.x);
            _animator.SetFloat("MoveY", moveInput.y);
            _animator.SetBool("IsMoving", true);

            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
            {
                _spriteRenderer.flipX = (moveInput.x < 0);
            }
        }
        else
        {
            // ВЫКЛЮЧАЕМ Animator, чтобы он не мешал менять спрайт через код!
            _animator.enabled = false;

            // И теперь спокойно ставим нужный Idle-спрайт
            if (Mathf.Abs(lastDir.x) > 0.1f && Mathf.Abs(lastDir.x) > Mathf.Abs(lastDir.y))
            {
                _spriteRenderer.sprite = _idleSide;
                _spriteRenderer.flipX = (lastDir.x < 0);
            }
            else if (lastDir.y > 0.1f)
            {
                _spriteRenderer.sprite = _idleBackward;
                _spriteRenderer.flipX = false;
            }
            else
            {
                _spriteRenderer.sprite = _idleForward;
                _spriteRenderer.flipX = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (InputBlocked)
        {
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