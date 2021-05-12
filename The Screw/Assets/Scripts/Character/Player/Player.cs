using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {
    Camera mainCamera;
    float distToGround;
    float turnSmoothVelocity;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        distToGround = GetComponentInChildren<Collider>().bounds.extents.y;
    }

    public bool IsGrounded(float extraDimension = 0.1f) {
        //return Physics.BoxCast(model.GetComponent<Collider>().bounds.center, model.GetComponent<Collider>().bounds.size / 2 - new Vector3(extraDimension, extraDimension, extraDimension), new Vector3(1f, -1f, 1f), model.transform.rotation, 2 * extraDimension, platformLayerMask.value);
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    void Move() {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float angleToZ = transform.eulerAngles.y * Mathf.Deg2Rad;
		float angleToX = (360 - transform.eulerAngles.y) * Mathf.Deg2Rad;

		Vector3 direction = new Vector3((h*Mathf.Cos(angleToX) + v*Mathf.Sin(angleToZ)), 0f, (h*Mathf.Sin(angleToX) + v*Mathf.Cos(angleToZ))).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
        transform.Rotate(0f, Input.GetAxis("Mouse X") * 3f, 0f);
        if (direction.magnitude > 0.3) {
			float newAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(model.transform.eulerAngles.y, newAngle, ref turnSmoothVelocity, 0.1f);
			model.transform.rotation = Quaternion.Euler(0f, angle, 0f);
		}
    }

    // Update is called once per frame
    void Update() {
        Move();
    }
}
