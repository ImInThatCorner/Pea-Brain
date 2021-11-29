using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isActive = true;
    public float speed = 1f;
    public float maxRotationAngle = 20f;
    public float minMoveDistance = 9f;

    public PlayerFOV playerFov;
    public Camera cam;

    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        if (!isActive) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 orientation = CalculateOrientation();
        float angle = CalculateOrientationAngle(orientation);

        // calculate directions based on 2D rotation matrix
        float xDirection = horizontal * Mathf.Cos(-angle) - vertical * Mathf.Sin(-angle);
        float yDirection = horizontal * Mathf.Sin(-angle) + vertical * Mathf.Cos(-angle);

        Vector3 direction = new Vector3(xDirection, 0, yDirection);

        // keeps speed constant for all directions
        if (horizontal != 0f && vertical != 0f)
            direction /= Mathf.Sqrt(2);

        Tilt(horizontal, vertical, orientation);

        if (orientation.magnitude < minMoveDistance && vertical > 0)
            return;    
        
        controller.Move(direction * speed);

    }

    private void Tilt(float horizontal, float vertical, Vector3 orientation)
    {
        float angle = Mathf.Rad2Deg * CalculateOrientationAngle(orientation);
        transform.rotation = Quaternion.Euler(maxRotationAngle * vertical, angle, -maxRotationAngle * horizontal);
    }

    private Vector3 CalculateOrientation()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerPosInScreen = cam.WorldToScreenPoint(transform.position);

        Vector3 orientation = (Vector2)(mousePos - playerPosInScreen);

        return orientation;
    }

    private float CalculateOrientationAngle(Vector3 v)
    {
        return Mathf.Atan2(v.x, v.y);
    }
}
