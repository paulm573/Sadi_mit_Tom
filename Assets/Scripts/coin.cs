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

    private void OnTriggerEnter(Collider other) {
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
        Debug.Log("hi");
    }
}
