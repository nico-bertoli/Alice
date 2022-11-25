using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Reads input using new input system
/// </summary>
public class InputManager : Singleton<InputManager>
{
    //======================================== fields
    public Vector2 MoveDirection { get; private set; }
    public bool IsMoving { get; set; }

    public bool IsUsingAbility { get; set; }    

    public bool IsDroppingDress { get; set; }   

    //======================================== methods
    public void Move(InputAction.CallbackContext context) {
        if (context.performed) {
            IsMoving = true;
            MoveDirection = context.ReadValue<Vector2>();
        }
        else if (context.canceled) {
            IsMoving = false;
        }
    }

    public void Ability(InputAction.CallbackContext context) {
        if (context.performed)
            IsUsingAbility = true;
        else if (context.canceled)
            IsUsingAbility = false;
    }

    public void DropDress(InputAction.CallbackContext context) {
        if (context.performed)
            IsDroppingDress = true;
        else if (context.canceled)
            IsDroppingDress = false;
    }
}