using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree2 : LSystem
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
            scaleRate = transform.localScale;
        }
    }

    public override void Init(int _depth, Transform _parent, Vector3 _scaleRate)
    {
        depth = _depth;
        parent = _parent;
        scaleRate.x = _scaleRate.x * 0.8f;
        scaleRate.y = _scaleRate.y * 0.9f;
        scaleRate.z = _scaleRate.z * 0.8f;

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
            float angle = Random.Range(0, 360);

            GameObject primary = Instantiate(obj, top, transform.rotation);
            primary.GetComponent<LSystem>().Init(depth+1, transform, scaleRate);
            primary.transform.Rotate(Vector3.up * Random.Range(0, 360));
            primary.transform.Rotate(Vector3.forward * 45);
            primary.transform.position += primary.transform.up * primary.transform.localScale.y;

            angle += Random.Range(90,150);

            primary = Instantiate(obj, top, transform.rotation);
            primary.GetComponent<LSystem>().Init(depth + 1, transform, scaleRate);
            primary.transform.Rotate(Vector3.up * angle);
            primary.transform.Rotate(Vector3.forward * 45);
            primary.transform.position += primary.transform.up * primary.transform.localScale.y;

            if (Random.Range(0,2) == 0) { return; }

            angle += Random.Range(90, 150);

            primary = Instantiate(obj, top, transform.rotation);
            primary.GetComponent<LSystem>().Init(depth + 1, transform, scaleRate);
            primary.transform.Rotate(Vector3.up * angle);
            primary.transform.Rotate(Vector3.forward * 45);
            primary.transform.position += primary.transform.up * primary.transform.localScale.y;
        }
    }
}
