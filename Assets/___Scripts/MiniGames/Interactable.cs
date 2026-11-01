using UnityEngine;

public enum MinigameType { Shapes, Wires, Fire, RapidClick } // Добавляем новые типы

public class Interactable : MonoBehaviour
{
    [Header("Minigame Settings")]
    [SerializeField] private MinigameType _minigameType;

    // Ссылки на панели мини-игр (перетащить в инспекторе)
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


        // Здесь можно менять материал детали (например, на красный, если сломана)
        GetComponent<Renderer>().material.color = isBroken ? Color.red : Color.white;

        if (isBroken) {
            _audioSource.PlayOneShot(_audioClip);
        }
    }

    // Вызывается из PlayerMovement по кнопке E
    public void Interact()
    {
        if (!isBroken) return; // Если деталь цела, ничего не делаем

        // Открываем нужную мини-игру
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
            panelToOpen.Open(this); // Передаем саму деталь в панель
        }
    }

    // Вызывается панелью, когда игрок прошел игру
    public void OnFixed()
    {
        isBroken = false;
        GetComponent<Renderer>().material.color = Color.white;
        GameFlowManager.Instance.OnPartFixed(this);

        _audioSource.Stop();
    }
}