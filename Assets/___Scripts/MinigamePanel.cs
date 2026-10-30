using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MinigamePanel : MonoBehaviour
{
    [Header("Root")]
    [SerializeField] private GameObject panelRoot;

    [Header("Blocker")]
    [SerializeField] private GameObject blocker;

    [Header("Cursor")]
    [SerializeField] private bool lockCursorOnClose = true;

    public bool IsOpen { get; private set; }

    void Awake()
    {
        if (panelRoot == null) panelRoot = gameObject;
        panelRoot.SetActive(false);

        if (blocker != null) blocker.SetActive(false);
    }

    public void Open()
    {
        if (IsOpen) return;
        IsOpen = true;

        if (blocker != null) blocker.SetActive(true);
        panelRoot.SetActive(true);

        PlayerMovement.InputBlocked = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Close()
    {
        if (!IsOpen) return;
        IsOpen = false;

        panelRoot.SetActive(false);
        if (blocker != null) blocker.SetActive(false);

        PlayerMovement.InputBlocked = false;

        if (lockCursorOnClose)
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
