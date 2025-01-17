﻿using UnityEngine;

/// <summary>
/// Monster movement and rotation.
/// </summary>
public class MonsterMotor : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float rotateSpeed = 7;
	[SerializeField] private float jumpForce = 5;

    new private Camera camera;
	new private Rigidbody rigidbody;
	private Animator animator;
	private Quaternion lastDirection;
	private int layerMask;

	private void Start()
	{
		camera = FindObjectOfType<Camera>();
		rigidbody = GetComponent<Rigidbody>();
		animator = GetComponentInChildren<Animator>();
		layerMask = LayerMask.GetMask("Player");
		layerMask = ~layerMask;
	}

	private void Update()
	{
		// Get user input
		Vector2 userInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		// Adjust off camera position
		Vector3 adjustedInput = (userInput.x * camera.transform.right) + (userInput.y * camera.transform.forward);

		// normalize the direction and remove the y component from the vector
		Vector3 moveDirection = new Vector3(adjustedInput.normalized.x, 0, adjustedInput.normalized.z);

		// Get a scalar for how much input is being sent
		var inputAmount = Mathf.Clamp01(Mathf.Abs(userInput.x) + Mathf.Abs(userInput.y));

		// Rotate player
		if (moveDirection != Vector3.zero) lastDirection = Quaternion.LookRotation(moveDirection);
		transform.rotation = Quaternion.Slerp(transform.rotation, lastDirection, Time.deltaTime * rotateSpeed);

		// Animator state
		//animator.SetBool("Moving", inputAmount != 0);
        animator.SetFloat("Forward", inputAmount);
        //animator.SetTrigger("Grab");

		// Move player
		rigidbody.MovePosition(transform.position + moveDirection * Time.deltaTime * inputAmount * moveSpeed);

		//Jump
		if (Input.GetButtonDown("Jump") && Mathf.Abs(rigidbody.velocity.y) < 0.01f)
		{
			Vector3 jumpDirection = new Vector3(moveDirection.x * inputAmount * 0.25f, 1, moveDirection.z * inputAmount * 0.25f);
			rigidbody.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
		}
	}
}
