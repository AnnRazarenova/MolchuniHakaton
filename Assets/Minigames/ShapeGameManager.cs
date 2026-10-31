
using UnityEngine;

public class ShapeGameManager : MonoBehaviour {
    public static ShapeGameManager singleton { get; private set; }

    public MinigamePanel minigamePanel;

    public RectTransform gameField;

    public RectTransform[] holes;
    public RectTransform[] shapes;

    public int placedCount = 0;

    private void Awake() {
        singleton = this;
    }

    private void OnEnable() {
        placedCount = 0;

        foreach (var item in shapes) {
            item.GetComponent<ShapeDrag>().isPlaced = false;
        }

        RandomizePositions(holes);
        RandomizePositions(shapes);
    }

    private void Update() {
        if (placedCount >= holes.Length) {
            placedCount = 0;
            minigamePanel.Close();
            // WIN MINIGAME
        }
    }

    private void RandomizePositions(RectTransform[] objects) {
        foreach (RectTransform obj in objects) {
            float x = Random.Range(-350f, 350f);
            float y = Random.Range(-180f, 180f);

            obj.anchoredPosition = new Vector2(x, y);
        }
    }
}
