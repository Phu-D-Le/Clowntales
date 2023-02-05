using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    private Vector2 move, mouseLook, joystickLook;
    private InputActionReference mouseAim, primaryFire;
    private Vector3 rotationTarget;
    private CharacterController characterController;
    public bool usingMouse = false, primaryFiring = false;
    public GunController gunPrimary;

    public void OnMouseAim(InputAction.CallbackContext context) // if the player click a mouse button -> player is using mouse -> lookAtMouse = true
    {
        if (context.performed)
        {
            usingMouse = true;
        }
        else
        {
            usingMouse = false;
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }
    public void OnMouseLook(InputAction.CallbackContext context)
    {
        mouseLook = context.ReadValue<Vector2>();
    }
    public void OnJoystickLook(InputAction.CallbackContext context)
    {
        joystickLook = context.ReadValue<Vector2>();
    }
    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            primaryFiring = true;
        }
        else
        {
            primaryFiring = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (usingMouse) // if player is using mouse, raycast from camera origin (middle of screen) to mouse position
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mouseLook);

            if (Physics.Raycast(ray, out hit))
            {
                rotationTarget = hit.point;
            }

            moveWhileAim();
        }

        else // else no mouse detected, use Controller joystick
        {
            if(joystickLook.x == 0 && joystickLook.y == 0)
            {
                movePlayer();
            }
            else
            {
                moveWhileAim();
            }
        }

        if (primaryFiring)
        {
            gunPrimary.isFiring = true;
        }

        else
        {
            gunPrimary.isFiring = false;
        }
    }

    public void movePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y);

        if (move.sqrMagnitude > 0.1f) { 

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        }

        characterController.Move(movement * movementSpeed * Time.deltaTime);
    }
    public void moveWhileAim() // when the character is moving and aiming, if else method to differentiation between K&M and Controller
    {
        if (usingMouse) // if player is using mouse
        {
            var lookPos = rotationTarget - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);

            Vector3 aimDirection = new Vector3(rotationTarget.x, 0f, rotationTarget.z);

            if (aimDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.15f);
            }
        }

        else // else player is using joystick
        {
            Vector3 aimDirection = new Vector3(joystickLook.x, 0f, joystickLook.y);

            if (aimDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(aimDirection), 0.15f);
            }
        }

        Vector3 movement = new Vector3(move.x, 0f, move.y);
        characterController.Move(movement * movementSpeed * Time.deltaTime);
    }
}
