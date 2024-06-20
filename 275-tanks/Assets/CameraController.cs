using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CameraController : MonoBehaviour
{
    [SerializeField] Transform playerTrfm;
    [SerializeField] Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - playerTrfm.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = playerTrfm.position + offset;
    }
}
