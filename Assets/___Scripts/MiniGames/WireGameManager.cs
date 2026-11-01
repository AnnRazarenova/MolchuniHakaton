using UnityEngine;

public class WireGameManager : MonoBehaviour
{
    public static WireGameManager singleton { get; private set; }

    private MinigamePanel _panel;

    [Header("References")]
    public RectTransform[] wires;
    public RectTransform[] sockets;

    [Header("Layout Settings (800x500 Back)")]
    public float wireX = -250f;
    public float socketX = 250f;

    public float wireMinY = -220f;
    public float wireMaxY = 220f;

    public float socketMinY = -220f;
    public float socketMaxY = 220f;

    [Header("Anti-Overlap")]
    [Tooltip("Минимальное расстояние по вертикали между сокетами")]
    public float minDistanceBetweenSockets = 70f;

    private int connectedCount = 0;

    private void Awake()
    {
        singleton = this;
    }

    public void Setup(MinigamePanel p)
    {
        _panel = p;
        connectedCount = 0;

        foreach (var item in wires)
        {
            item.GetComponent<WireDrag>().isPlaced = false;
        }

        // --- ГЕНЕРАЦИЯ ПРОВОДОВ ---
        int count = wires.Length;
        float totalHeight = wireMaxY - wireMinY;
        float sectionHeight = totalHeight / count;

        for (int i = 0; i < count; i++)
        {
            float minYForThisWire = wireMinY + (sectionHeight * i);
            float maxYForThisWire = wireMinY + (sectionHeight * (i + 1));

            float y = Random.Range(minYForThisWire, maxYForThisWire);
            wires[i].anchoredPosition = new Vector2(wireX, y);
        }

        // --- ГЕНЕРАЦИЯ СОКЕТОВ С ЗАЩИТОЙ ОТ НАЛОЖЕНИЙ ---
        int socketCount = sockets.Length;
        float socketTotalHeight = socketMaxY - socketMinY;
        float socketSectionHeight = socketTotalHeight / socketCount;

        for (int i = 0; i < socketCount; i++)
        {
            float sectionStart = socketMinY + (socketSectionHeight * i);
            float sectionEnd = socketMinY + (socketSectionHeight * (i + 1));

            // Оставляем отступы внутри секции (половина от минимального расстояния)
            float padding = minDistanceBetweenSockets / 2f;
            float safeStart = sectionStart + padding;
            float safeEnd = sectionEnd - padding;

            // Проверка, что зона не стала "перевернутой" (если секция слишком мала)
            if (safeStart > safeEnd) safeStart = safeEnd;

            float y = Random.Range(safeStart, safeEnd);
            sockets[i].anchoredPosition = new Vector2(socketX, y);
        }
    }

    public void OnWirePlaced()
    {
        connectedCount++;
        if (connectedCount >= wires.Length)
        {
            connectedCount = 0;
            if (_panel != null)
            {
                _panel.OnMinigameWon();
            }
        }
    }
}