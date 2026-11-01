using UnityEngine;
using UnityEngine.UI;

public enum MinigameType { Shapes, Wires, Fire, RapidClick }

public class Interactable : MonoBehaviour
{
    [Header("Minigame Settings")]
    [SerializeField] private MinigameType _minigameType;

    [SerializeField] private MinigamePanel _shapesPanel;
    [SerializeField] private MinigamePanel _wiresPanel;
    [SerializeField] private MinigamePanel _firePanel;
    [SerializeField] private MinigamePanel _rapidClickPanel;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    private bool isBroken = false;

    public MinigameType MinigameType => _minigameType;

    // Вызывается менеджером
    public void SetFixed(bool fixedState)
    {
        isBroken = !fixedState;

        // ВМЕСТО СТАРОЙ СТРОКИ ВЫЗЫВАЕМ НОВЫЙ МЕТОД
        SetColor(isBroken ? Color.red : Color.white);

        if (isBroken && _audioSource != null && _audioClip != null)
        {
            _audioSource.PlayOneShot(_audioClip);
        }
    }

    // Вызывается из PlayerMovement по кнопке E
    public void Interact()
    {
        if (!isBroken) return;

        MinigamePanel panelToOpen = null;
        switch (_minigameType)
        {
            case MinigameType.Shapes: panelToOpen = _shapesPanel; break;
            case MinigameType.Wires: panelToOpen = _wiresPanel; break;
            case MinigameType.Fire: panelToOpen = _firePanel; break;
            case MinigameType.RapidClick: panelToOpen = _rapidClickPanel; break;
        }

        if (panelToOpen != null)
        {
            panelToOpen.Open(this);
        }
    }

    // Вызывается панелью, когда игрок прошел игру
    public void OnFixed()
    {
        isBroken = false;

        // ВМЕСТО СТАРОЙ СТРОКИ ВЫЗЫВАЕМ НОВЫЙ МЕТОД
        SetColor(Color.white);

        GameFlowManager.Instance.OnPartFixed(this);

        if (_audioSource != null) _audioSource.Stop();
    }

    // НОВЫЙ МЕТОД: Ищет картинку внутри (SpriteRenderer или UI Image) и красит её
    private void SetColor(Color color)
    {
        // Находим все 3D/2D спрайты внутри
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (var sprite in sprites)
        {
            sprite.color = color;
        }

        // Находим все UI картинки внутри
        Image[] images = GetComponentsInChildren<Image>();
        foreach (var img in images)
        {
            img.color = color;
        }
    }
}