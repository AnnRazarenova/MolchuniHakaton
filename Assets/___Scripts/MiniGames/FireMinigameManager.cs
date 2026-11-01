using UnityEngine;

public class FireMinigameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _fires;
    [SerializeField] private int _minFires = 2;
    [SerializeField] private int _maxFires = 4;
    // Ссылка на огнетушитель больше не нужна для сброса позиции, 
    // но можно оставить, если пригодится в будущем.
    // [SerializeField] private FireExtinguisherDrag _extinguisher; 

    private MinigamePanel _panel;
    private Interactable _currentPart;
    private bool _isFinished = false;

    public void Setup(MinigamePanel p, Interactable part)
    {
        _panel = p;
        _currentPart = part;
        _isFinished = false;

        // УБРАЛИ: if (_extinguisher != null) _extinguisher.ResetPosition();

        // Выключаем все огни
        foreach (var f in _fires) if (f != null) f.SetActive(false);

        int fireCount = Random.Range(_minFires, _maxFires + 1);

        // Перемешиваем
        for (int i = 0; i < _fires.Length; i++)
        {
            GameObject temp = _fires[i];
            int randomIndex = Random.Range(i, _fires.Length);
            _fires[i] = _fires[randomIndex];
            _fires[randomIndex] = temp;
        }

        // Включаем нужное количество
        for (int i = 0; i < fireCount && i < _fires.Length; i++)
        {
            if (_fires[i] != null) _fires[i].SetActive(true);
        }
    }

    private void Update()
    {
        if (_isFinished) return;

        int activeFires = 0;
        foreach (var f in _fires)
        {
            if (f != null && f.activeSelf) activeFires++;
        }

        if (activeFires == 0)
        {
            _isFinished = true;
            if (_panel != null) _panel.OnMinigameWon();
        }
    }
}