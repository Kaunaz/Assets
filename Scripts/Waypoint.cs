using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{

    void Start()
    {
        // SpawnWaypoint();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuming the player object has a "Player" tag
        {
            Debug.Log("Triggered");
            Destroy(transform.parent.gameObject);
        }
    }

}