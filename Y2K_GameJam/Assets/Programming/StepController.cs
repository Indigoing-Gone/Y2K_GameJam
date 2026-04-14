using System;
using System.Collections;
using UnityEngine;

public class StepController : MonoBehaviour
{
    static public event Action OnStepTaken;

    [SerializeField] private float stepInterval;

    private void Start()
    {
        StartCoroutine(Step());
    }

    private IEnumerator Step()
    {
        while (true)
        {
            yield return new WaitForSeconds(stepInterval);
            Debug.Log("Step taken");
            OnStepTaken?.Invoke();
        }
    }
}