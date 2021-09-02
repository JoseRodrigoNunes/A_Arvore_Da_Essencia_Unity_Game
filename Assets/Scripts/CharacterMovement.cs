using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;
    PlayerCombat playerCombat;
    private Vector3 rollDir;
    public int Speed = 8;
    int isWalkingHash;

    Vector2 currentMovementInput;
    public Vector3 currentMovement;
    bool isMovementPressed;
    float rotationFactorPerFrame = 10.0f;

    public float dashSpeed;
    public float dashTime;
    public float nextDashTime = .0f;
    public bool canDash;

    // Start is called before the first frame update
    private void Awake()
    {
        canDash = true;
        playerCombat = GetComponent<PlayerCombat>();
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");

        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Dash.started += onDashInput;
    }

    void onDashInput(InputAction.CallbackContext ctx)
    {
        //Debug.Log("Dash was pressed!");
        if (Time.time >= nextDashTime && isMovementPressed && Time.time >= playerCombat.nextAttackTime && canDash)
        {
            //Debug.Log("Dash was performed!");
            animator.SetTrigger("IsDashing");
            StartCoroutine(Dash());
            nextDashTime = Time.time + 1f;
        }

    }

    IEnumerator Dash()
    {
        float startTime = Time.time;

        while(Time.time < startTime + dashTime)
        {
            characterController.Move(transform.forward * dashSpeed * Time.deltaTime);

            yield return null;
        }
    }
    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }
    public void setSpeed(int speed, float animationSpeed)
    {
        Speed = speed;
        animator.SetFloat("WalkingSpeed", animationSpeed);
    }

    void handleAnimation(){
        bool isWalking = animator.GetBool(isWalkingHash);

        if(isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }
        if(!isMovementPressed && isWalking){ 
            animator.SetBool(isWalkingHash, false);
        }
    }



    void handleRotation()
    {
        Vector3 PositionToLookAt;
        PositionToLookAt.x = currentMovement.x;
        PositionToLookAt.y = 0.0f;
        PositionToLookAt.z = currentMovement.z;

        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed) { 
            Quaternion targetRotation = Quaternion.LookRotation(PositionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }


    void handleGravity()
    {
        if (characterController.isGrounded)
        {
            float groundedGravity = -.05f;
            currentMovement.y = groundedGravity;
        }
        else
        {
            float gravity = -9.8f;
            currentMovement.y = gravity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        handleRotation();
        handleAnimation();
        handleGravity();
        characterController.Move(currentMovement * Time.deltaTime * Speed);
    }

    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}