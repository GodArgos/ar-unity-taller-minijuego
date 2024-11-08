using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PrefabCreator : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector3 prefabOffset;

    private GameObject player;
    private ARTrackedImageManager imageManager;

    private void OnEnable()
    {
        imageManager = gameObject.GetComponent<ARTrackedImageManager>();
        imageManager.trackedImagesChanged += OnImageChanged;
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (ARTrackedImage image in args.added)
        {
            player = Instantiate(playerPrefab, image.transform);
            player.transform.position += prefabOffset;
        }
    }
}
