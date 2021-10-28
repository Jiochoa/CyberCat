using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
//--------------------------------------------------------------------
//Small class to move an object back and forth along a path. Used for
//moving platforms in levels
//--------------------------------------------------------------------

public class Mover: MonoBehaviour
{
    [Header("Mover")]
    [SerializeField] float m_MovementSpeed = 0.0f;
    [SerializeField] float m_MaxDistance = 0.0f;
    [SerializeField] Vector3 m_Direction;
    [SerializeField] AnimationCurve m_MoveCurve = null; 
    [SerializeField] bool m_MoveAlignedToRotation = false; 

    protected float m_Time;
    protected Vector3 m_StartPosition;

    public Vector3 upperBound;
    public Vector3 lowerBound;


    public bool platfromIsEnabled { get; set; }

    void Start()
    {
        m_StartPosition = transform.position;
        platfromIsEnabled = true;
    }

    void FixedUpdate()
    {

        if(platfromIsEnabled)
        {
            m_Time += Time.fixedDeltaTime * m_MovementSpeed;
            float offset = m_MoveCurve.Evaluate(m_Time) * m_MaxDistance * 2.0f - m_MaxDistance;
            Vector3 t_Direction = m_MoveAlignedToRotation ? transform.rotation * m_Direction.normalized : m_Direction.normalized;
            transform.position = m_StartPosition + offset * t_Direction.normalized;
        } 


    }

    public void EnablePlatform()
    {
        platfromIsEnabled = true;
    }

    public void DisablePlatform()
    {
        platfromIsEnabled = false;
    }



    void OnDrawGizmos()
    {
        Vector3 start = transform.position;

        if (Application.isPlaying)
        {
            start = m_StartPosition;
        }
        Gizmos.color = Color.green;
        Vector3 direction = m_Direction.normalized;
        upperBound = start + direction * m_MaxDistance; 
        lowerBound = start - direction * m_MaxDistance;
        Gizmos.DrawLine(start - direction * m_MaxDistance, start + direction * m_MaxDistance);

        Gizmos.matrix = Matrix4x4.TRS(start - direction * m_MaxDistance, transform.rotation, transform.localScale);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        Gizmos.matrix = Matrix4x4.TRS(start + direction * m_MaxDistance, transform.rotation, transform.localScale);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        Gizmos.matrix = Matrix4x4.identity;
    }
}
