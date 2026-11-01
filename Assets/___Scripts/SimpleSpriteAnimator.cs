using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SimpleSpriteAnimator : MonoBehaviour
{
    [Header("Настройки анимации")]
    [Tooltip("Перетащите сюда ваши картинки по порядку")]
    public Sprite[] frames;

    private SpriteRenderer _spriteRenderer;
    private int _currentFrame = 0;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (frames.Length > 0)
        {
            _spriteRenderer.sprite = frames[0];
        }
    }

    // Убираем Update с таймером, он больше не нужен!

    // Публичный метод: вызываем его, когда игрок победил в мини-игре
    public void NextFrame()
    {
        if (frames.Length == 0) return;

        _currentFrame++;

        // Если дошли до конца, начинаем сначала (или останавливаемся на последнем)
        if (_currentFrame >= frames.Length)
        {
            _currentFrame = 0; // Зацикливаем
            // Если не хотите зацикливать, а остановиться на последнем кадре:
            // _currentFrame = frames.Length - 1;
        }

        _spriteRenderer.sprite = frames[_currentFrame];
    }
}