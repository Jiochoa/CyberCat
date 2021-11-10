using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIJoystickActionInputOverride : MonoBehaviour
{
	//TODO-----------------------------------
	//Fix:apply thiese actions to action Joystick
	// Jump (Vertical up)
	// Hold Jump (Vertical down)
	// Grab/Push (DdeadZone)
	// Horizontal front/ horizontal back ?
	//TODO-----------------------------------

    [SerializeField] PlayerInput m_PlayerInput = null;
    [SerializeField] Joystick joystickAction;
    //[SerializeField] string[] m_InputOverrideName;
    Vector2 m_CurrentInput;
    bool isInDeadZone = false;
	bool isJump = false;
	bool isLongJump = false;


	public void joystickButtonDown()
	{
		isInDeadZone =
			Mathf.Abs(joystickAction.Horizontal) < joystickAction.DeadZone
			&& Mathf.Abs(joystickAction.Vertical) < joystickAction.DeadZone;

		if (!isInDeadZone)
		{
			//print("is outside dead zone");
			m_CurrentInput = new Vector2(joystickAction.Horizontal, joystickAction.Vertical);
			//m_PlayerInput.GetDirectionInput(m_InputOverrideName[0]).SetOverride(true, m_CurrentInput);


			if(m_CurrentInput.y > joystickAction.DeadZone) // if pointing up
			{
				m_PlayerInput.GetButton("Jump").SetOverride(true, true);
				isJump = true;
			}
			else if (m_CurrentInput.y < joystickAction.DeadZone) // if pointing down
            {
				print("Holding for Hold Jump...");
				m_PlayerInput.GetButton("LongJump").SetOverride(true, true);
				isLongJump = true;
			}

			print("outside DZ");

		}
		else
		{
			m_CurrentInput = Vector2.zero;
			//m_PlayerInput.GetDirectionInput(m_InputOverrideName[0]).SetOverride(true, m_CurrentInput);
			print("is Grabing....");
			print("inside DZ");

		}

	}

	public void joystickButtonUp()
	{
		m_CurrentInput = Vector2.zero;
		//m_CurrentInput.x = 0.0f;

		if(isJump == true)
        {
			m_PlayerInput.GetButton("Jump").SetOverride(true, false);
			isJump = false;
        } else if (isLongJump == true)
        {
			m_PlayerInput.GetButton("LongJump").SetOverride(true, false);
			isLongJump = false;
        }


		//if(m_PlayerInput.GetButton("Jump").)


		print("joystick Down deselected " + m_CurrentInput);
	}


}
