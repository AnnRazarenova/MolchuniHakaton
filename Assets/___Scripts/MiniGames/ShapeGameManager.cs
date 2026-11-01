using UnityEngine;

public class ShapeGameManager : MonoBehaviour
{
    public static ShapeGameManager singleton { get; private set; }

    private MinigamePanel _panel;

    public RectTransform gameField;
    public RectTransform[] holes;
    public RectTransform[] shapes;

    private int placedCount = 0;

    // Настройки зоны рандома
    public float minX = -350f;
    public float maxX = 350f;
    public float minY = -180f;
    public float maxY = 180f;

    private void Awake()
    {
        singleton = this;
    }

    public void Setup(MinigamePanel p)
    {
        _panel = p;
        placedCount = 0;

        foreach (var item in shapes)
        {
            item.GetComponent<ShapeDrag>().isPlaced = false;
        }

        RandomizePositions(holes);
        RandomizePositions(shapes);
    }

    // Вызывается из ShapeDrag
    public void OnShapePlaced()
    {
        placedCount++;

        if (placedCount >= holes.Length)
        {
            placedCount = 0;
            if (_panel != null)
            {
                _panel.OnMinigameWon();
            }
        }
    }

    private void RandomizePositions(RectTransform[] objects)
    {
        foreach (RectTransform obj in objects)
        {
            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY, maxY);
            obj.anchoredPosition = new Vector2(x, y);
        }
    }
}