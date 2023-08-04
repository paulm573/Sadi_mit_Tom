using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.one * rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        this.gameObject.SetActive(false);
        Debug.Log("hi");
    }
}
