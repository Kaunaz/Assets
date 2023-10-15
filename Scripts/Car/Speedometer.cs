using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Speedometer : MonoBehaviour
{
    public Rigidbody ship;

    public TMP_Text speedometer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float speed = ship.velocity.magnitude * 3.6f;

        if(speedometer != null){
            speedometer.text = speed.ToString() + " km/h";
        }


    }
}
