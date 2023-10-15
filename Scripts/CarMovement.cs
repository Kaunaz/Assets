using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carMovement : MonoBehaviour
{
    public float moveSpeed = 10;
    public float rotationSpeed = 100;
    public float maxSpeed = 10;
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()    {

        rb = GetComponent <Rigidbody>();
        
    }
    // Update is called once per frame
    void Update()
    {
        // Captura la entrada del usuario
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        rb.angularVelocity = Vector3.zero;

        // Aplicar rotación
        transform.Rotate(Vector3.up * horizontalInput * rotationSpeed * Time.deltaTime);

        // Aplicar movimiento hacia adelante
        if (verticalInput > 0)
        {
            Vector3 moveDirection = transform.forward * moveSpeed * Time.deltaTime;
            transform.position += moveDirection;
        }

        // Limitar la velocidad máxima
        float currentSpeed = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity).z;
        if (currentSpeed > maxSpeed)
        {
            float speedDifference = currentSpeed - maxSpeed;
            Vector3 brakeForce = transform.forward * speedDifference;
            GetComponent<Rigidbody>().AddForce(-brakeForce);
        }
    }
}