using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenade : MonoBehaviour
{
    private GameObject smokeEffect;
    private bool hasActivated = false;
    private float destructionTimer = 0.0f;
    private const float destructionDelay = 3.0f;  // Timpul de a?teptare pentru distrugerea grenadei (3 secunde)

    void Start()
    {
        smokeEffect = transform.Find("smoke").gameObject;

        if (smokeEffect != null)
        {
            smokeEffect.SetActive(false); // Asigur?-te c? efectul de fum este dezactivat la început
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!hasActivated && smokeEffect != null)
        {
            smokeEffect.SetActive(true);  // Activeaz? efectul de fum
            hasActivated = true;

            // Semnal?m paznicul s? investigheze grenada
            GuardAI guard = FindObjectOfType<GuardAI>();  // G?sim primul paznic din scen?
            if (guard != null)
            {
                guard.GoToGrenade(transform.position, gameObject);  // Trimitem loca?ia grenadei ?i obiectul ei
            }
        }
    }

    void Update()
    {
        // Dac? grenadele sunt activate ?i vrem s? le distrugem dup? un anumit timp
        if (hasActivated)
        {
            destructionTimer += Time.deltaTime;

            // Distruge grenada dup? 3 secunde
            if (destructionTimer >= destructionDelay)
            {
                Destroy(gameObject);  // Distruge obiectul grenad?
            }
        }
    }
}
