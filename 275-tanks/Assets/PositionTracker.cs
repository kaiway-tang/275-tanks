using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTracker : MonoBehaviour
{
    public Transform trfm;
    int trackerID;
    private void Start()
    {
        trfm = transform;
        cachedPositions = new Vector3[20];

        trackerID = GameManager.AddTracker(GetComponent<PositionTracker>());
    }

    private void OnDestroy()
    {
        GameManager.KillTracker(trackerID);
    }

    private void FixedUpdate()
    {
        HandlePositionPredicting();
    }

    public Vector3 PredictedPosition(float time)
    {
        int cacheIndices = Mathf.RoundToInt(time / 0.1f);

        if (cacheIndices < 1) { return transform.position; }
        else if (cacheIndices < 21)
        {
            cacheIndices = newCacheIndex - cacheIndices;
            if (cacheIndices < 0) { cacheIndices += cachedPositions.Length; }
            return transform.position + transform.position - cachedPositions[cacheIndices];
        }
        else
        {
            return transform.position + transform.position - (cachedPositions[newCacheIndex] * time / 2);
        }
    }

    int newCacheIndex;
    int positionCacheTimer;
    [SerializeField] Vector3[] cachedPositions;
    public void HandlePositionPredicting()
    {
        if (positionCacheTimer > 0) { positionCacheTimer--; return; }
        positionCacheTimer = 5;

        cachedPositions[newCacheIndex] = transform.position;
        newCacheIndex++;
        newCacheIndex = newCacheIndex % cachedPositions.Length;
    }
}