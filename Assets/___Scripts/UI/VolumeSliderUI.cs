using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

[RequireComponent(typeof(Slider))]
public class VolumeSliderUI : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private string _mixerParamName = "MusicVol";

    [Header("Visual Settings")]
    [SerializeField] private Image _soundIcon;
    [SerializeField] private Sprite _iconOn;
    [SerializeField] private Sprite _iconOff;

    private Slider _slider;

    void Start()
    {
        _slider = GetComponent<Slider>();

        // Настраиваем слайдер: от 0 до 100, только целые шаги
        _slider.minValue = 0f;
        _slider.maxValue = 100f;
        _slider.wholeNumbers = true;

        _slider.onValueChanged.AddListener(OnSliderChanged);
        OnSliderChanged(_slider.value);
    }

    void OnSliderChanged(float value)
    {
        // Округляем до ближайшего десятка (10, 20, 30...)
        float steppedValue = Mathf.Round(value / 10f) * 10f;

        // Если значение изменилось после округления, принудительно ставим его на слайдер
        // (это заставит ползунок "прыгнуть" на нужное деление)
        if (Mathf.Abs(value - steppedValue) > 0.01f)
        {
            _slider.SetValueWithoutNotify(steppedValue);
        }

        // Переводим 100 шагов в диапазон от 0 до 1
        float normalizedValue = steppedValue / 100f;

        // Защита от log(0)
        normalizedValue = Mathf.Max(normalizedValue, 0.0001f);

        // Перевод в децибелы для AudioMixer
        float db = Mathf.Log10(normalizedValue) * 20f;
        _audioMixer.SetFloat(_mixerParamName, db);

        // Переключение иконки (если ползунок на 0)
        if (_soundIcon != null)
        {
            _soundIcon.sprite = steppedValue <= 0f ? _iconOff : _iconOn;
        }
    }
}