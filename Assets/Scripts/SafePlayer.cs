using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class SafePlayer : MonoBehaviour
{


    [SerializeField]
    public Transform Player;

    private void OnTriggerEnter(Collider other) {
        Debug.Log("bla");
        Player.transform.position = new(this.transform.position.x, this.transform.position.y + 20, this.transform.position.z);

    }
}
