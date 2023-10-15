using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public Rigidbody sphere;

    public float hoverHeight, fwdAccel, bckAccel, maxSpeed, turnStrenght, rollAngle, driftTurn, driftSmooth, AngleChange;
    public Transform Child;
    public Transform Turn;
    private float VInput, HInput;
    private bool driftInput;
    private float roll;

    public TrailRenderer[] Trails;

    public float trailThreshold;

    private float driftFactor, driftRoll;

    public Transform[] rayPoints;
    public Transform hoverPoint;
    private RaycastHit hit;

    private float terrainHeight;
    private float lastHitDistance;

    public float hoverF;
    public float gravityF;

    [Header("PID variables")]

    public float Kp = 1;
    public float Ki = 0;
    public float Kd = 0.1f;

    private float prevError;
    private float P, I, D;

    void Awake()
    {
        sphere.transform.parent = null;
    }

    void Update()
    {
        InputHandler();
        DrawRay();
        TrailHandler();
        VerticalMove();
        HorizontalMove();
        TerrainAllign();
        transform.position = sphere.transform.position;
    }

    void FixedUpdate(){
        HoverPID();
    }

    void DrawRay(){
        foreach (var rayPoint in rayPoints)
        {
            Vector3 start = rayPoint.position;
            Vector3 end = rayPoint.position - transform.up * terrainHeight;
            Debug.DrawLine(start, end, Color.black, 0f);

            start = end;
            end = start + hit.normal * 5f;
            Debug.DrawLine(start, end, Color.green, 0f); 
        }
    }

    void TerrainAllign(){   // Parent
        RaycastHit allign;
        Physics.Raycast(hoverPoint.position, -transform.up, out allign);
        Vector3 projection = Vector3.ProjectOnPlane(transform.forward, allign.normal);
        Quaternion rotation = Quaternion.LookRotation(projection, allign.normal);

        sphere.MoveRotation(Quaternion.Slerp(sphere.rotation, rotation, Time.deltaTime * AngleChange * sphere.velocity.magnitude/2));
        transform.rotation = sphere.transform.rotation;
    }

    void HoverPID(){
        RaycastHit hit1;
        Physics.Raycast(hoverPoint.position, -transform.up, out hit1);
        float correction = hoverHeight - hit1.distance;

        float PIDcorrection = GetOutput(correction, Time.fixedDeltaTime);

        Vector3 hoverForce = hit1.normal.normalized * hoverF * PIDcorrection;
        Vector3 gravityForce = -hit1.normal.normalized * gravityF * hit1.distance;

        sphere.AddForce(hoverForce, ForceMode.Acceleration);
        sphere.AddForce(gravityForce, ForceMode.Acceleration);

        Debug.DrawLine(hoverPoint.position, hoverPoint.position - transform.up * hit1.distance, Color.black, 0f);
        Debug.DrawLine(hoverPoint.position - transform.up * hit1.distance, ((hoverPoint.position - transform.up * hit1.distance) - hit1.normal*5), Color.red, 0f);
     }


    public float GetOutput(float currentError, float dt)
    {
        P = currentError;
        I += P * dt;
        D = (P - prevError) / dt;
        prevError = currentError;
    
        return P*Kp + I*Ki + D*Kd;
    }
    
    void TrailHandler(){
        if(sphere.velocity.magnitude > trailThreshold){
            foreach (var Trail in Trails)
            {
                Trail.emitting = true;
            }
        }
        else{
            foreach (var Trail in Trails)
            {
                Trail.emitting = false;
            }
        }
    }

    void VerticalMove(){    // Parent
        if (VInput > 0){
            sphere.AddForce(Turn.forward * fwdAccel * VInput, ForceMode.Acceleration);
        }
        if (VInput < 0)
        {
            sphere.AddForce(Turn.forward * bckAccel * VInput, ForceMode.Acceleration);
        }
        Debug.DrawLine(transform.position, (transform.position + Child.forward*10) , Color.magenta, 0f);
    }

    void InputHandler(){
        VInput = Input.GetAxis("Vertical");
        HInput = Input.GetAxis("Horizontal");
        driftInput = Input.GetKey(KeyCode.Space);
    }

    void HorizontalMove(){
        if(driftInput){
            driftRoll = Mathf.MoveTowards(driftRoll, 1.0f, Time.deltaTime * driftSmooth);
            driftFactor = driftTurn;
        }
        else{
            driftRoll = Mathf.MoveTowards(driftRoll, 0.0f, Time.deltaTime * driftSmooth);
            driftFactor = 1;
        }

        roll = Mathf.Lerp(0,rollAngle, ((Mathf.Abs(HInput)) + (Mathf.Abs(HInput) * driftRoll)) / 2) * -Mathf.Sign(HInput);
        Turn.Rotate(0f, HInput * turnStrenght * driftFactor * Time.deltaTime,0f, Space.Self);
        Child.localRotation = Quaternion.Euler(0f,0f,roll);
    }

}
