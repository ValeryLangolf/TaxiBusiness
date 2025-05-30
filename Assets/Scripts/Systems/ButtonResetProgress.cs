using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonResetProgress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private DataService dataService;
    [SerializeField] private Image _fill;
    [SerializeField] private float _holdingTime = 3f;

    private Coroutine _fillCoroutine;

    private void Awake() =>
        ResetFill();

    public void ResetGame() =>
        dataService.ResetGame();

    public void OnPointerDown(PointerEventData eventData) =>
        _fillCoroutine ??= StartCoroutine(FillProgress());

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_fillCoroutine != null)
        {
            StopCoroutine(_fillCoroutine);
            _fillCoroutine = null;
            ResetFill();
        }
    }

    private IEnumerator FillProgress()
    {
        float currentTime = 0f;

        while (currentTime < _holdingTime)
        {
            currentTime += Time.deltaTime;
            _fill.fillAmount = Mathf.Clamp01(currentTime / _holdingTime);

            yield return null;
        }

        ResetGame();
        ResetFill();
        _fillCoroutine = null;
    }

    private void ResetFill() =>
        _fill.fillAmount = 0f;
}