using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeJar : MonoBehaviour
{
    [SerializeField] private List<GameObject> fragments = new List<GameObject>();
    public float explosionForce = 0.1f; // The force to apply to the fragments
    public float explosionRadius = 0.05f; // The radius within which the force is applied

    public void Explode(Vector3 hitPoint)
    {
        foreach (GameObject fragment in fragments)
        {
            // Get the Rigidbody component of the fragment
            Rigidbody rb = fragment.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Calculate the direction from the hit point to the fragment
                Vector3 direction = (fragment.transform.position - hitPoint).normalized;

                // Apply explosion force to the fragment
                rb.AddExplosionForce(explosionForce, hitPoint, explosionRadius);
            }
        }
    }
}
