using UnityEngine;
using UnityEngine.InputSystem;

public class MinigamePanel : MonoBehaviour
{
    [Header("Root")]
    [SerializeField] private GameObject _panelRoot;

    [Header("Blocker")]
    [SerializeField] private GameObject _blocker;

    [Header("Cursor")]
    [SerializeField] private bool _lockCursorOnClose = true;

    public bool IsOpen { get; private set; }

    void Awake()
    {
        if (_panelRoot == null) _panelRoot = gameObject;
        _panelRoot.SetActive(false);

        if (_blocker != null) _blocker.SetActive(false);
    }

    public void Open()
    {
        if (IsOpen) return;
        IsOpen = true;

        if (_blocker != null) _blocker.SetActive(true);
        _panelRoot.SetActive(true);

        PlayerMovement.InputBlocked = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Close()
    {
        if (!IsOpen) return;
        IsOpen = false;

        _panelRoot.SetActive(false);
        if (_blocker != null) _blocker.SetActive(false);

        PlayerMovement.InputBlocked = false;

        if (_lockCursorOnClose)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        if (!IsOpen) return;

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            Close();
    }
}
