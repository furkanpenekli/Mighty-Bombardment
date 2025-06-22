using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [Header("Obstacle Settings")]
    [SerializeField] private float minInterval = 30f;
    [SerializeField] private float maxInterval = 60f;
    [SerializeField] private float activeDuration = 2f;

    [Header("Obstacle Fences")]
    [SerializeField] private List<Building> fences = new List<Building>();

    private Coroutine cycleRoutine;

    private void OnEnable()
    {
        cycleRoutine = StartCoroutine(ActivationCycle());
    }

    private void OnDisable()
    {
        if (cycleRoutine != null)
            StopCoroutine(cycleRoutine);
    }

    private IEnumerator ActivationCycle()
    {
        while (true)
        {
            float nextInterval = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(nextInterval);

            ActivateObstacle();
            yield return new WaitForSeconds(activeDuration);
            DeactivateObstacle();
        }
    }

    private void ActivateObstacle()
    {
        foreach (var fence in fences)
        {
            if (fence != null)
            {
                fence.gameObject.SetActive(true);
                fence.ResetHealth();
            }
        }
    }

    private void DeactivateObstacle()
    {
        foreach (var fence in fences)
        {
            if (fence != null)
                fence.gameObject.SetActive(false);
        }
    }
}
