using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PositionTracker[] trackers;
    static bool[] alive;
    static int assignID;

    public static GameManager self;
    // Start is called before the first frame update

    private void Awake()
    {
        self = this;
        alive = new bool[trackers.Length];
        assignID = 0;
    }

    void Start()
    {
        
    }

    public static int AddTracker(PositionTracker tracker)
    {
        self.trackers[assignID] = tracker;
        alive[assignID] = true;
        assignID++;

        return assignID - 1;
    }

    public static void KillTracker(int ID)
    {
        alive[ID] = false;
    }

    static float minDist = 0, currentDist = 0;
    static int nearestID;
    public static PositionTracker NearestTarget(Vector3 position)
    {
        minDist = 99999999;
        nearestID = -1;

        for (int i = 0; i < self.trackers.Length; i++)
        {
            if (alive[i])
            {
                currentDist = Vector3.SqrMagnitude(position - self.trackers[i].trfm.position);
                if (currentDist > 0.3 && currentDist < minDist)
                {
                    minDist = currentDist;
                    nearestID = i;
                }
            }            
        }

        if (nearestID > -1 && alive[nearestID])
        {
            return self.trackers[nearestID];
        }
        return null;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log(NearestTarget(Vector3.zero));
        }
    }
}
