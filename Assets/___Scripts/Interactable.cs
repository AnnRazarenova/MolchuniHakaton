using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Panel")]
    [Tooltip("Панель, которая откроется при взаимодействии")]
    [SerializeField] private MinigamePanel panelToOpen;

    [Header("Prompt (optional)")]
    [Tooltip("Текст подсказки, если будешь показывать UI-подсказку")]
    [SerializeField] private string promptText = "Нажми E";

    public string PromptText => promptText;

    public void Interact()
    {
        if (panelToOpen == null)
        {
            Debug.LogWarning($"[Interactable] {name}: panelToOpen не назначен", this);
            return;
        }

        panelToOpen.Open();
    }
}
