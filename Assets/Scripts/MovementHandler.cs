using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    public float movementSpeed;

    [SerializeField] float deltaThreshold;
    [SerializeField] float sensitivity;
    [SerializeField] float minZ;
    [SerializeField] float maxZ;

    private Vector3 firstPosition;
    private Vector3 currentTouchPosition;
    private Rigidbody rb;
    private float finalZ;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        ResetValues();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if(GameManager.Instance.canStart)
        {
            Move();
            Shifting();
        }
        else
        {
            Stop();
        }
    }

    private void Stop()
    {
        rb.velocity = Vector3.zero;
    }

    private void Move()
    {
        Vector3 velocity = rb.velocity;
        velocity.x = movementSpeed;
        rb.velocity = velocity;
    }

    private void Shifting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            currentTouchPosition = Input.mousePosition;
            Vector2 touchDelta = (currentTouchPosition - firstPosition);

            if (firstPosition == currentTouchPosition)
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0f);

            finalZ = transform.position.z;

            if (Mathf.Abs(touchDelta.y) >= deltaThreshold)
                finalZ = (transform.position.z + (touchDelta.x * sensitivity));

            rb.position = new Vector3(transform.position.x, transform.position.y, finalZ);
            rb.position = new Vector3(rb.position.x, rb.position.y, Mathf.Clamp(rb.position.z, minZ, maxZ));

            firstPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            ResetValues();
        }
    }

    private void ResetValues()
    {
        rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        firstPosition = Vector2.zero;
        finalZ = 0f;
        currentTouchPosition = Vector2.zero;
    }
}
