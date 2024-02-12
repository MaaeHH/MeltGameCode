using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamcoinParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystemPrefab;
    private int numberOfFires = 5;

    private void Start()
    {
        // Start the firing process
        StartCoroutine(FireParticleSystem());
    }

    private System.Collections.IEnumerator FireParticleSystem()
    {
        for (int i = 0; i < numberOfFires; i++)
        {

            particleSystemPrefab.Emit(1);

            // Random delay between 0.1 and 0.4 seconds
            float delay = Random.Range(0.1f, 0.4f);

            // Wait for the delay before proceeding to the next iteration
            yield return new WaitForSeconds(delay);
        }
    }
}
