using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree1 : LSystem
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
            primary.GetComponent<LSystem>().Init(depth + 4, transform, scaleRate);
            primary.transform.position += primary.transform.up * primary.transform.localScale.y;

            while (attempts < 30)
            {
                if (Random.Range(0, 10) == 0)
                {
                    GameObject branch = Instantiate(obj, top, Quaternion.identity);
                    branch.GetComponent<LSystem>().Init(depth + 1, transform, scaleRate);
                    branch.transform.Rotate(Vector3.up * Random.Range(0,360));
                    branch.transform.Rotate(Vector3.forward * Random.Range(60, 80));
                    branch.transform.position += branch.transform.up * (branch.transform.localScale.y * .8f);
                    attempts += 10;
                }

                attempts++;
            }
        }
    }
}
