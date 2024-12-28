using UnityEngine;
using System.Collections;


public class FadeEffect : MonoBehaviour
{
    private Renderer towerRenderer;
    private Material towerMaterial;

    void Start()
    {
        towerRenderer = GetComponent<Renderer>();
        if (towerRenderer != null)
        {
            towerMaterial = towerRenderer.material;
        }
    }

    public IEnumerator FadeIn(float duration)
    {
        if (towerMaterial == null) yield break;

        Color color = towerMaterial.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / duration); // Gradually increase alpha
            towerMaterial.color = color;
            yield return null;
        }

        color.a = 1f; // Ensure it's fully visible
        towerMaterial.color = color;
    }

    public IEnumerator FadeOut(float duration)
    {
        if (towerMaterial == null) yield break;

        Color color = towerMaterial.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(1f - (elapsedTime / duration)); // Gradually decrease alpha
            towerMaterial.color = color;
            yield return null;
        }

        color.a = 0f; // Ensure it's fully invisible
        towerMaterial.color = color;
    }

    public IEnumerator SmoothScale(Vector3 targetScale, float duration)
    {
        Vector3 initialScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration); // Smoothly interpolate scale
            yield return null;
        }

        transform.localScale = targetScale; // Ensure it ends at the exact target scale
    }

}
