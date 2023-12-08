using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MR_PlayerMovementScript : MonoBehaviour
{
    PlayerInputsScript playerInputs;
    [SerializeField] CharacterController characterController;

    [Header("Custscene")]
    [SerializeField] bool inCutscene;

    [Header("Player Speed")]
    [SerializeField] float speed;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    Vector3 playerVelocity;

    [Header("Jump Power")]
    [SerializeField] float jumpHeight = 1f;
    [SerializeField]float gravity = -9.8f;

    public void SetCutscene(bool cutscene)
    {
        inCutscene = cutscene;
    }
    
    public bool GetCutscene()
    {
        return inCutscene;
    }

    private void Awake()
    {
        playerInputs = new PlayerInputsScript();
        playerInputs.Player.Enable();

        speed = walkSpeed;

        playerInputs.Player.Jump.performed += Jump;
        playerInputs.Player.Sprint.performed += SetSprintSpeed;
        playerInputs.Player.Sprint.canceled += SetWalkSpeed;
    }

    private void Update()
    {
        ProcessMovement();
    }

    private void ProcessMovement()
    {
        if (inCutscene)
        {
            return;
        }

        Vector2 inputVector = playerInputs.Player.Movement.ReadValue<Vector2>();
        Vector3 movementDir = new Vector3(inputVector.x, 0, inputVector.y);

        playerVelocity.y += gravity * Time.deltaTime;
        
        if(characterController.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2;
        }

        Vector3 moveInputVal = transform.TransformDirection(movementDir) * speed;
        
        playerVelocity.x = moveInputVal.x;
        playerVelocity.z = moveInputVal.z;
        
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    private void SetSprintSpeed(InputAction.CallbackContext context)
    {
        speed = runSpeed;
    }

    private void SetWalkSpeed(InputAction.CallbackContext context)
    {
        speed = walkSpeed;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if(inCutscene)
        {
            return;
        }

        if(characterController.isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
}
