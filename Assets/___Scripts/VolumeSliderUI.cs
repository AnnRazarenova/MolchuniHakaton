using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

[RequireComponent(typeof(Slider))]
public class VolumeSliderUI : MonoBehaviour
{
    public AudioMixer audioMixer;
    public string mixerParamName = "MusicVol";

    public Image soundIcon;
    public Sprite iconOn;
    public Sprite iconOff;

    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();

        // Настраиваем слайдер: от 0 до 100, только целые шаги
        slider.minValue = 0f;
        slider.maxValue = 100f;
        slider.wholeNumbers = true;

        slider.onValueChanged.AddListener(OnSliderChanged);
        OnSliderChanged(slider.value);
    }

    void OnSliderChanged(float value)
    {
        // Переводим 100 шагов в диапазон от 0 до 1
        float normalizedValue = value / 100f;

        // Защита от log(0)
        normalizedValue = Mathf.Max(normalizedValue, 0.0001f);

        // Перевод в децибелы для AudioMixer
        float db = Mathf.Log10(normalizedValue) * 20f;
        audioMixer.SetFloat(mixerParamName, db);

        // Переключение иконки (если ползунок на 0)
        if (soundIcon != null)
        {
            soundIcon.sprite = value <= 0f ? iconOff : iconOn;
        }
    }
}