using System.Collections;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance { get; private set; }

    [Header("Parts (Детали)")]
    [Tooltip("Все детали, которые могут ломаться (вешаем сюда все пустышки/фигуры)")]
    [SerializeField] private Interactable[] _allParts;

    [Header("Settings")]
    [SerializeField] private float _minBreakDelay = 2f;
    [SerializeField] private float _maxBreakDelay = 5f;

    private Interactable currentBrokenPart;
    private int lastMinigameIndex = -1; // Чтобы не было больше 2 повторений подряд
    private int sameMinigameCount = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // В начале игры все детали целы
        foreach (var part in _allParts)
        {
            part.SetFixed(true);
        }

        // Запускаем первый цикл поломки
        StartCoroutine(BreakLoop());
    }

    private IEnumerator BreakLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_minBreakDelay, _maxBreakDelay));

            if (currentBrokenPart == null)
            {
                ChooseNextPartToBreak();
            }
        }
    }

    private void ChooseNextPartToBreak()
    {
        // Логика выбора, чтобы не было >2 повторений одной группы
        int randomIndex = Random.Range(0, _allParts.Length);
        Interactable chosenPart = _allParts[randomIndex];
        int minigameIndex = (int)chosenPart.MinigameType;

        if (minigameIndex == lastMinigameIndex)
        {
            if (sameMinigameCount >= 2)
            {
                ChooseNextPartToBreak(); // Если выпало 3 раза подряд - перевыбираем
                return;
            }
            sameMinigameCount++;
        }
        else
        {
            lastMinigameIndex = minigameIndex;
            sameMinigameCount = 1;
        }

        // Ломаем деталь
        currentBrokenPart = chosenPart;
        currentBrokenPart.SetFixed(false);
        Debug.Log($"Сломалась деталь: {currentBrokenPart.name} (Миниигра: {chosenPart.MinigameType})");
    }

    // Вызывается из скрипта Interactable, когда игрок починил деталь
    public void OnPartFixed(Interactable part)
    {
        if (currentBrokenPart == part)
        {
            currentBrokenPart = null;
        }
    }
}