using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakOnHit : MonoBehaviour
{
    [SerializeField] private GameObject brokenPrefab;
    [SerializeField] private GameObject canvas;
    [SerializeField] private AudioClip clip;
    private GameObject hitPrefab;
    private float timer = 2f;
    private SpawnManager spawnManager;

    private void OnEnable()
    {
        canvas.SetActive(false);
        spawnManager = FindObjectOfType<SpawnManager>();
    }

    private void Update()
    {
        if (hitPrefab != null)
        {
            if (timer <= 0)
            {
                Destroy(hitPrefab);
                Destroy(gameObject);
            }
            else
            {
                timer -= Time.deltaTime;
                canvas.transform.LookAt(Camera.main.transform.position);
                canvas.transform.rotation = Quaternion.Euler(0f, canvas.transform.rotation.eulerAngles.y + 180, 0f);
            }
        }
    }

    public void Hit(Vector3 hitPoint)
    {
        if (brokenPrefab != null)
        {
            hitPrefab = Instantiate(brokenPrefab, transform.position, Quaternion.identity);
            hitPrefab.GetComponent<ExplodeJar>().Explode(hitPoint);
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            canvas.SetActive(true);
            GetComponent<AudioSource>().PlayOneShot(clip);
            spawnManager.UpdateText();
            spawnManager.currentObjects--;
        }
    }
}
