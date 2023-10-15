using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderControll : MonoBehaviour
{
    public Transform Turn;
    public Transform Roll;
    public Transform Colliders;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Colliders.rotation = Turn.rotation;
    }
}
