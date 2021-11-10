using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIJoystickDirectionInputOverride : MonoBehaviour
{
	//TODO---------------------------------
	//Bug: When joystick is pressed and released outside the main joystick space
	//the character keeps moving.
	//Fix: ???
	//TODO---------------------------------

	[SerializeField] PlayerInput m_PlayerInput = null;
	[SerializeField] Joystick joystickMove;
	[SerializeField] string m_InputOverrideName = "";
	Vector2 m_CurrentInput = Vector2.zero;
	bool isInDeadZone = false;



	public void joystickButtonDown()
    {
		isInDeadZone = 
			Mathf.Abs(joystickMove.Horizontal) < joystickMove.DeadZone 
			&& Mathf.Abs(joystickMove.Vertical) < joystickMove.DeadZone;

		if(!isInDeadZone)
        {
			//print("is outside dead zone");
			m_CurrentInput = new Vector2(joystickMove.Horizontal, joystickMove.Vertical);
			m_PlayerInput.GetDirectionInput(m_InputOverrideName).SetOverride(true, m_CurrentInput);
		}
		else
        {
			m_CurrentInput = Vector2.zero;
			m_PlayerInput.GetDirectionInput(m_InputOverrideName).SetOverride(true, m_CurrentInput);
			//print("is inside deadzone");
        }

	}

	public void joystickButtonUp()
    {
		m_CurrentInput = Vector2.zero;
		//m_CurrentInput.x = 0.0f;
		m_PlayerInput.GetDirectionInput(m_InputOverrideName).SetOverride(true, m_CurrentInput);
		//print("joystick Down deselected");
	}


	//TODO---------------------------------
	//Bug: When joystic is pressed and released outside the main joystic space
	//the character keeps moving.
	//Fix: ???
	//TODO---------------------------------

}
