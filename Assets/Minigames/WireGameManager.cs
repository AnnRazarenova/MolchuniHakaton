using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireGameManager : MonoBehaviour {
    public static WireGameManager singleton { get; private set; }

    public MinigamePanel minigamePanel;

    public RectTransform gameField;

    public RectTransform[] wires;
    private List<Vector3> wiresPos = new List<Vector3>();

    public int connectedCount = 0;

    private void Awake() {
        singleton = this;
        foreach (var item in wires) {
            wiresPos.Add(item.position);
        }
    }

    private void OnEnable() {
        connectedCount = 0;

        foreach (var item in wires) {
            item.GetComponent<WireDrag>().isPlaced = false;
        }

        int r = Random.Range(0, 25);
        for (int i = 0; i < wires.Length; i++) {
            wires[i].position = wiresPos[(i + r) % wires.Length];
        }
    }

    private void Update() {
        if (connectedCount >= wires.Length) {
            connectedCount = 0;
            minigamePanel.Close();
            // WIN MINIGAME
        }
    }
}
