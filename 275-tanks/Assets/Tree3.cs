using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree3 : LSystem
{
    [SerializeField] bool manualStep;
    [SerializeField] Vector3 bottom, half, top;
    // Start is called before the first frame update
    bool firstTick;
    int attempts;
    new void Start()
    {
        base.Start();

        if (!parent)
        {
            scaleRate = transform.localScale / maxDepth;
        }
    }

    public override void Init(int _depth, Transform _parent, Vector3 _scaleRate)
    {
        depth = _depth;
        parent = _parent;
        scaleRate.x = _scaleRate.x * 7f;
        scaleRate.y = _scaleRate.y * Random.Range(25f,45f);
        scaleRate.z = _scaleRate.z * 7f;

        transform.localScale = scaleRate;
    }

    private void Update()
    {
        if (!firstTick && manualStep && Input.GetMouseButtonDown(0))
        {
            GenerateBranch();
        }
    }

    private void FixedUpdate()
    {
        if (!firstTick && !manualStep)
        {
            GenerateBranch();
        }
    }

    void GenerateBranch()
    {
        bottom = transform.position; half = transform.position; top = transform.position;

        bottom.y = transform.position.y - transform.localScale.y;
        top = transform.position + transform.up * transform.localScale.y;

        firstTick = true;

        if (depth < maxDepth)
        {
            
            GameObject primary = Instantiate(obj, top, Quaternion.identity);
            primary.transform.localScale = transform.localScale * 0.9f;
            //primary.GetComponent<LSystem>().Init(depth + 1, null, scaleRate);
            primary.GetComponent<Tree3>().depth = depth + 1;
            primary.transform.position += primary.transform.up * primary.transform.localScale.y;
            

            int angle = 0;

            while (angle < 360)
            {
                angle += Random.Range(7, 14);
                if (Random.Range(0, 2) == 0)
                {
                    GameObject branch = Instantiate(obj, top + new Vector3(0,1,0)* Random.Range(-0.5f,0.5f) * transform.localScale.y, Quaternion.identity);
                    branch.GetComponent<LSystem>().Init(depth + 99, transform, scaleRate);
                    branch.transform.Rotate(Vector3.up * angle);
                    branch.transform.Rotate(Vector3.forward * Random.Range(75,105));
                    branch.transform.position += branch.transform.up * (branch.transform.localScale.y * .8f);
                    //attempts += 4;
                }
            }
        }
    }
}
