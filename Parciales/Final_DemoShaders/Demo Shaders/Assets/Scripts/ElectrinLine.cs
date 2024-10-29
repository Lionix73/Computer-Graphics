using System.Collections;
using UnityEngine;

public class ElectrinLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 step;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private Material material;
    [SerializeField] private int pointCount;
    [SerializeField] private float toggleInterval = 0.05f;
    [SerializeField] private float randomness = 0.5f;
    [SerializeField] private float thickness = 0.3f;
    

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.widthMultiplier = 0.1f;
        lineRenderer.positionCount = pointCount;
        lineRenderer.material = material;
        

        StartCoroutine(ToggleLightning());
    }

    void Update()
    {
        UpdateThickness(thickness);
    }

    private IEnumerator ToggleLightning()
    {
        while (true)
        {
            lineRenderer.enabled = !lineRenderer.enabled;

            if (lineRenderer.enabled)
            {
                UpdateLightningPositions();
            }

            yield return new WaitForSeconds(toggleInterval);
        }
    }

    private void UpdateLightningPositions()
    {
        lineRenderer.SetPosition(0, startPoint.transform.position);
        lineRenderer.SetPosition(pointCount - 1, endPoint.transform.position);

        step = (endPoint.transform.position - startPoint.transform.position) / (pointCount - 1);

        for (int i = 1; i < pointCount - 1; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-randomness, randomness),
                Random.Range(-randomness, randomness),
                Random.Range(-randomness, randomness)
            );
            lineRenderer.SetPosition(i, startPoint.transform.position + step * i + randomOffset);
        }
    }

    private void UpdateThickness(float value)
    {
        lineRenderer.widthMultiplier = value;
    }
}