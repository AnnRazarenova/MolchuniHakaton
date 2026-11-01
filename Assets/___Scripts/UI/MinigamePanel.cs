using UnityEngine;
using UnityEngine.InputSystem;

public class MinigamePanel : MonoBehaviour
{
    [SerializeField] private GameObject _panelRoot;
    [SerializeField] private GameObject _blocker;

    public FireMinigameManager fireManager;
    public ShapeGameManager shapesManager;
    public WireGameManager wiresManager;

    private Interactable currentPart;

    void Awake()
    {
        if (_panelRoot == null) _panelRoot = gameObject;
        _panelRoot.SetActive(false);
        if (_blocker != null) _blocker.SetActive(false);
    }
    public SimpleSpriteAnimator animator; // Перетащите сюда объект с анимацией в инспекторе

    // В методе OnMinigameWon():
    public void OnMinigameWon()
    {
        if (currentPart != null) currentPart.OnFixed();
        Close();

        // МЕНЯЕМ КАДР
        if (animator != null) animator.NextFrame();
    }
    public void Open(Interactable part)
    {
        currentPart = part;
        if (_blocker != null) _blocker.SetActive(true);
        _panelRoot.SetActive(true);

        PlayerMovement.InputBlocked = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Выключаем все дочерние панели
        if (shapesManager != null) shapesManager.gameObject.SetActive(false);
        if (wiresManager != null) wiresManager.gameObject.SetActive(false);
        if (fireManager != null) fireManager.gameObject.SetActive(false);

        // Включаем нужную и вызываем Setup
        switch (part.MinigameType)
        {
            case MinigameType.Shapes:
                if (shapesManager != null)
                {
                    shapesManager.gameObject.SetActive(true);
                    shapesManager.Setup(this); // <--- ОБЯЗАТЕЛЬНО
                }
                break;
            case MinigameType.Wires:
                if (wiresManager != null)
                {
                    wiresManager.gameObject.SetActive(true);
                    wiresManager.Setup(this); // <--- ОБЯЗАТЕЛЬНО
                }
                break;
            case MinigameType.Fire:
                if (fireManager != null)
                {
                    fireManager.gameObject.SetActive(true);
                    fireManager.Setup(this, part);
                }
                break;
        }
    }

    public void Close()
    {
        _panelRoot.SetActive(false);
        if (_blocker != null) _blocker.SetActive(false);

        PlayerMovement.InputBlocked = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentPart = null;
    }

    void Update()
    {
        if (!_panelRoot.activeSelf) return;
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            Close();
    }
}