using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationDelay : MonoBehaviour
{

    [SerializeField] private float timeToActivate = 1f;

    [Header("COMPONENTS")]
    [SerializeField] private List<GameObject> listComponentes;

    private void OnEnable()
    {
        StartCoroutine(Activation(true));
    }

    private void OnDisable()
    {
        ActivationAction(false);
    }

    private IEnumerator Activation(bool activate)
    {
        yield return new WaitForSeconds(timeToActivate);

        ActivationAction(activate);
    }

    private void ActivationAction(bool activate)
    {
        foreach (GameObject component in listComponentes)
        {
            component.SetActive(activate);
        }
    }
}
