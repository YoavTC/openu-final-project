using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class CanvasShake : MonoBehaviour
{
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float magnitude = 10f;
    [SerializeField] private float scalePunchStrength;

    private RectTransform contentTransform;

    [Button]
    public void Shake()
    {
        contentTransform = GetComponent<RectTransform>();
        contentTransform.DOPunchScale(Vector3.one * scalePunchStrength, duration);
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        Vector3 originalPosition = contentTransform.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            contentTransform.anchoredPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        contentTransform.anchoredPosition = originalPosition;
    }
}