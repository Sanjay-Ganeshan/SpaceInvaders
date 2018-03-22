using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Replaces the mouse with a Crosshair
/// </summary>
public class CrosshairFollow : MonoBehaviour {

    const string INPUT_CONTROLLER_TOGGLE = "ToggleController";

    const string INPUT_CONTROLLER_HORIZONTAL = "CrosshairHorizontal";

    const string INPUT_CONTROLLER_VERTICAL = "CrosshairVertical";

    public InputMode ControlMode = InputMode.KEYBOARD_AND_MOUSE;

    private int NUM_MODES;

    public float CrosshairSpeed = 10.0f;

    Vector3 ControllerTarget;

	// Use this for initialization
	void Start () {
        // Hide the hardware cursor, the crosshair will
        // serve that purpose
        Cursor.visible = false;
        NUM_MODES = Enum.GetValues(typeof(InputMode)).Length;
        ControllerTarget = Vector3.zero;
	}
	
	// LateUpdate is called once per frame
	void LateUpdate () {
        // Move this object to wherever the 
        // mouse is pointing.
        Cursor.visible = false;
        this.transform.position = GetTarget();
	}

    void Update()
    {
        if(Input.GetButtonDown(INPUT_CONTROLLER_TOGGLE))
        {
            NextMode();
        }
        if(this.ControlMode == InputMode.CONTROLLER)
        {
            this.ControllerTarget += (CrosshairSpeed * new Vector2(Input.GetAxis(INPUT_CONTROLLER_HORIZONTAL), Input.GetAxis(INPUT_CONTROLLER_VERTICAL)).ToVector3());
        }
    }

    void NextMode()
    {
        Vector3 prevTarget = GetTarget();
        this.ControlMode = (InputMode)(((int)this.ControlMode + 1) % NUM_MODES);
        if(this.ControlMode == InputMode.CONTROLLER)
        {
            this.ControllerTarget = Camera.main.WorldToScreenPoint(prevTarget);
        }
    }

    public Vector2 GetTarget()
    {
        switch(this.ControlMode)
        {
            case InputMode.CONTROLLER:
                return Camera.main.ScreenToWorldPoint(this.ControllerTarget).To2DVector3();              

            default:
            case InputMode.KEYBOARD_AND_MOUSE:
                return Common.GetMousePosition3D();
        }
    }

    public void ModifyTarget(Vector2 offset)
    {
        if(this.ControlMode == InputMode.CONTROLLER)
        {

        }
    }
}
