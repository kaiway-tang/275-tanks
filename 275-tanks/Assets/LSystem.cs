using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystem : MonoBehaviour
{
    [SerializeField] protected int depth, maxDepth;
    [SerializeField] protected GameObject obj;
    [SerializeField] protected Transform parent;
    [SerializeField] protected Vector3 scaleRate;
    // Start is called before the first frame update
    protected void Start()
    {
        if (!parent)
        {
            scaleRate = transform.localScale / maxDepth;
        }
    }

    public void Init(int _depth, Transform _parent, Vector3 _scaleRate)
    {
        depth = _depth;
        parent = _parent;
        scaleRate = _scaleRate;

        //scaleRate.y = 0;
        transform.localScale = _parent.localScale - scaleRate;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
