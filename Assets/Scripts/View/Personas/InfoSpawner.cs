using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class InfoSpawner : MonoBehaviour
{
    public InfoSphere _infoPrefab;
    public ARRaycastManager _raycastManager;
    public ARAnchorManager _anchorManager;

    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    private List<InfoSphere> _infoSpherePool = new List<InfoSphere>();
    private const int PoolInitialSize = 5;

    void Start()
    {
        if (_raycastManager == null)
        {
            _raycastManager = FindObjectOfType<ARRaycastManager>();
        }

        if (_anchorManager == null)
        {
            _anchorManager = FindObjectOfType<ARAnchorManager>();
        }
        InitializePool();
    }

    void InitializePool()
    {
        for (int i = 0; i < PoolInitialSize; i++)
        {
            InfoSphere pooledObject = Instantiate(_infoPrefab);
            pooledObject.gameObject.SetActive(false);
            _infoSpherePool.Add(pooledObject);
        }
    }

    public void SpawnInfoSphere(Persona persona)
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        if (_raycastManager.Raycast(screenCenter, _hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = _hits[0].pose;
            Vector3 spawnPosition = hitPose.position;
            Quaternion spawnRotation = hitPose.rotation;

            ARAnchor anchor = _anchorManager.AddAnchor(new Pose(spawnPosition, spawnRotation));
            if (anchor != null)
            {
                try
                {
                    InfoSphere spawnedObject = GetPooledInfoSphere();
                    spawnedObject.transform.position = spawnPosition;
                    spawnedObject.transform.rotation = spawnRotation;
                    spawnedObject.gameObject.SetActive(true);
                    spawnedObject.SetPersona(persona);
                    spawnedObject.transform.parent = anchor.transform;
                }
                catch (NoPoolObjectsAvailableException e)
                {
                    Debug.Log(e.Message);
                    return;
                }
            }
        }
    }

    private InfoSphere GetPooledInfoSphere()
    {
        foreach (var pooledObject in _infoSpherePool)
        {
            if (!pooledObject.gameObject.activeInHierarchy)
            {
                return pooledObject;
            }
        }
        throw new NoPoolObjectsAvailableException();
    }
}