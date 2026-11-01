using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FireExtinguisherDrag : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private Image _sprayFoamImage;
    private Canvas canvas;
    private RectTransform rectTransform;
    private bool isDragging = false;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();

        if (_sprayFoamImage != null)
            _sprayFoamImage.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        if (_sprayFoamImage != null)
            _sprayFoamImage.gameObject.SetActive(true);

        GetComponent<AudioSource>().Play();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        // Двигаем огнетушитель (пена, как дочерний объект, двигается вместе с ним)
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        // Теперь проверяем огонь не по мышке, а по позиции ПЕНЫ
        if (_sprayFoamImage != null)
        {
            // Переводим мировые координаты пены в координаты экрана
            Vector2 foamScreenPos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, _sprayFoamImage.transform.position);
            CheckForFire(foamScreenPos);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        if (_sprayFoamImage != null)
            _sprayFoamImage.gameObject.SetActive(false);

        GetComponent<AudioSource>().Stop();
    }

    private void CheckForFire(Vector2 screenPosition)
    {
        // Создаем событие с координатами пены (или мышки, если пены нет)
        PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = screenPosition };
        var results = new System.Collections.Generic.List<RaycastResult>();

        // Пускаем луч из позиции пены
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            // Если луч попал в огонь
            if (result.gameObject.CompareTag("Fire"))
            {
                // Тушим
                result.gameObject.SetActive(false);
            }
        }
    }
}