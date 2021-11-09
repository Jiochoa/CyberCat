using UnityEngine;
using System.Collections;
//--------------------------------------------------------------------
//Long Jump is a movement ability
//If the Long Jump input is pressed, and the character is on the ground, charge the Jump untill for
//input is released then apply that force to the jump
//--------------------------------------------------------------------
public class LongJumpModule : GroundedControllerAbilityModule
{
    //TODO---------------------------
    //Bug: The line doesnt come up when you press the LongJump Button and
    // drag it 
    //Fix: Set up lineRenderer...
    //
    //Bug: Character needs to stop when charging the LongJump 
    //Fix: ??
    //TODO---------------------------



    [SerializeField] float m_AdditionalJumpVelocity = 0.0f;
    [SerializeField] LineRenderer lr;
    Camera myCamera;
    Vector2 m_CurrentHoldForce;
    Vector2 force;
    Vector3 startPoint;
    Vector3 endPoint;
    private Vector2 minPower = new Vector2(-8, -8); // TODO: Why 8?
    private Vector2 maxPower = new Vector2(8, 8);


    //Called whenever this module is started (was inactive, now is active)
    protected override void StartModuleImpl()
    {
        // start long jump
        // TODO: Step 1 -> Stop Motion
        //m_CurrentHoldForce = Vector2.zero;
        startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        startPoint.z = 15f;
        
        //print("started Long Jump ");

    }

    //Walk across the floor, but with different values for speed and friction
    //Can be used to slow down the movement when crouching
    //Called for every fixedupdate that this module is active
    public override void FixedUpdateModule()
    {
        // calculate long jump force
        Vector3 curentPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        curentPoint.z = 15;
        //StartLongJumpLine(startPoint, curentPoint);
        //animator.SetBool("isCharging", true);
        StartLongJumpLine(startPoint, curentPoint);
        //print("startPoint = " + startPoint + "currentPoint " + curentPoint);
    }

    //Called whenever this module is ended (was active, now is inactive)
    protected override void EndModuleImpl()
    {
        // Apply long jump force
        endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        startPoint.z = 15f;

        force = startPoint - endPoint;
        Vector3 clampedForce = Vector3.ClampMagnitude(force, maxPower.x) * m_AdditionalJumpVelocity;

        /*
        force = new Vector2(
            Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x),
            Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y));
        force.x += 8;
        */

        m_CurrentHoldForce = force * m_AdditionalJumpVelocity;

        m_CharacterController.Jump(clampedForce);

        //startPoint = Vector3.zero;
        //endPoint = Vector3.zero;
        //m_ControlledCollider.UpdateWithVelocity(m_ControlledCollider.GetVelocity());
        //EndLongJumpLine();
        //canCatchInput = true;
        //animator.SetBool("isCharging", false);

        //animator.SetBool("isJumping", true);
        //print("finish long jump force = " + clampedForce);
        EndLongJumpLine();
        //timer = 0f;
    }




    //Character needs to be on the floor and pressing the crouch button, or moving down with arrow keys/analogue stick
    //Query whether this module can be active, given the current state of the character controller (velocity, isGrounded etc.)
    //Called every frame when inactive (to see if it could be) and when active (to see if it should not be)
    public override bool IsApplicable()
    {
        

        if (m_ControlledCollider.IsGrounded() &&
            ((DoesInputExist("LongJump") && GetButtonInput("LongJump").m_IsPressed) || GetDirInput("Move").m_Direction == DirectionInput.Direction.Down ||
            (!m_ControlledCollider.CanBeResized(m_ControlledCollider.GetDefaultLength(), CapsuleResizeMethod.FromBottom))))
        {
            return true;
        }
        return false;
    }
    //Query whether this module can be deactivated without bad results (clipping etc.)
    public override bool CanEnd()
    {
        if (m_ControlledCollider != null)
        {
            return m_ControlledCollider.CanBeResized(m_ControlledCollider.GetDefaultLength(), CapsuleResizeMethod.FromBottom);

        }
        return true;
    }
    //Get the name of the animation state that should be playing for this module. 
    public override string GetSpriteState()
    {
        if (Mathf.Abs(m_ControlledCollider.GetVelocity().x) < 0.0001f)
        {
            return "CrouchIdle";
        }
        else
        {
            return "Crouch";
        }
    }

    public void StartLongJumpLine(Vector3 startPoint, Vector3 endPoint)
    {
        lr.positionCount = 2;
        Vector3[] points = new Vector3[2];
        points[0] = startPoint;
        points[1] = endPoint;

        lr.SetPositions(points);
    }
    public void EndLongJumpLine()
    {
        lr.positionCount = 0;
    }
}
