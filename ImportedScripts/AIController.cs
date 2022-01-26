﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpaceShip))]
public class AIController : MonoBehaviour
{
    public enum AIBehaivour
    {
        Null,
        Patrol
    }
    [SerializeField] private AIBehaivour m_AIBehaivour;
    [SerializeField] private AIPointPatrol m_PatrolPoint;

    [Range(0.0f, 1.0f)]
    [SerializeField] private float m_NavigationLinear;

    [Range(0.0f, 1.0f)]
    [SerializeField] private float m_NavigationAngular;    

    [SerializeField] private float m_RandomSelectMovePointTime;
    [SerializeField] private float m_FindNewTargetTime;
    [SerializeField] private float m_ShootDelay;
    [SerializeField] private float m_EvadeRayLength;

    private SpaceShip m_SpaceShip;
    private Vector3 m_MovePosition;
    private Destructible m_SelectedTarget;

    private Timer m_RandomizeDirectionTimer;
    private Timer m_FireTimer;
    private Timer m_FindNewTargetTimer;


    private void Start() {
        m_SpaceShip = GetComponent<SpaceShip>();
        InitTimers();
    }

    private void Update() {
        UpdateTimers();
        UpdateAI();
    }

    private void UpdateAI()
    {
        if(m_AIBehaivour == AIBehaivour.Patrol)
        {
            UpdateBehaviourPatrol();
        }
    }

    public void UpdateBehaviourPatrol()
    {
        ActionFindNewMovePosition();
        ActionControlShip();
        ActionFindNewAttackTarget();
        ActionEvadeCollision();
        ActionFire();
    }

    private void ActionFindNewMovePosition()
    {
        if(m_AIBehaivour == AIBehaivour.Patrol)
        {
            if(m_SelectedTarget != null)
            {
                m_MovePosition = m_SelectedTarget.transform.position + transform.forward;
            }
            else
            {
                if(m_PatrolPoint != null)
                {
                    bool isInsidePatrolZone = (m_PatrolPoint.transform.position - transform.position).sqrMagnitude < m_PatrolPoint.Radius * m_PatrolPoint.Radius;

                    if(isInsidePatrolZone == true)
                    {
 
                            GetNewMovePoint();
                    }
                    else
                    {
                        m_MovePosition = m_PatrolPoint.transform.position;
                    }
                }
            }
        }
    }

    private void ActionEvadeCollision()
    {
        if(Physics2D.Raycast(transform.position, transform.up, m_EvadeRayLength) == true)
        {
            m_MovePosition = transform.position + transform.right * 100.0f;

            if(Physics2D.Raycast(transform.position, transform.right, m_EvadeRayLength) == true)
            {
                m_MovePosition = transform.position + -transform.right * 100.0f;
            }

        }
    }

    private void ActionControlShip()
    {
        m_SpaceShip.ThrustControl = m_NavigationLinear;
        m_SpaceShip.TorqueControl = ComputeAliginTorqueNormalized(m_MovePosition, m_SpaceShip.transform) * m_NavigationAngular;
    }

    private static float ComputeAliginTorqueNormalized(Vector3 targetPosition, Transform ship)
    {
        Vector2 localTargetPosition = ship.InverseTransformPoint(targetPosition);
        float angle = Vector3.SignedAngle(localTargetPosition, Vector3.up, Vector3.forward);
        angle = Mathf.Clamp(angle, -45, 45) / 45;
        return -angle;
    }

    private void ActionFindNewAttackTarget()
    {
        if(m_FindNewTargetTimer != null)
        {
            if(m_FindNewTargetTimer.IsFinished == true)
            {
                m_SelectedTarget = FindNearestDestructibleTarget();
                m_FindNewTargetTimer.Start(m_ShootDelay);
            }
        }
    }

    private void ActionFire()
    {
        if(m_SelectedTarget != null)
        {
            if(m_FireTimer.IsFinished == true)
            {
                //m_SpaceShip.Fire(TurretMode.Primary);
                m_FireTimer.Start(m_ShootDelay);
            }
        }
    }

    private Destructible FindNearestDestructibleTarget()
    {
        float maxDist = float.MaxValue;
        Destructible potentialTarget = null;
        foreach (var v in Destructible.AllDestructibles)
        {
            if(v.GetComponent<SpaceShip>() == m_SpaceShip) continue;

            if(v.TeamId == Destructible.TeamIdNeutral) continue;

            if(v.TeamId == m_SpaceShip.TeamId) continue;

            float dist = Vector2.Distance(m_SpaceShip.transform.position, v.transform.position);
            if(dist < maxDist)
            {
                maxDist = dist;
                potentialTarget = v;
            }
            
        }
        return potentialTarget;
    }

    #region Timers

    private void InitTimers()
    {
        m_RandomizeDirectionTimer = new Timer(m_RandomSelectMovePointTime);
        m_FireTimer = new Timer(m_ShootDelay);
        m_FindNewTargetTimer = new Timer(m_FindNewTargetTime);
    }

    private void UpdateTimers()
    {
        m_RandomizeDirectionTimer.RemoveTime(Time.deltaTime);
        m_FireTimer.RemoveTime(Time.deltaTime);
        m_FindNewTargetTimer.RemoveTime(Time.deltaTime);
    }

    protected virtual void GetNewMovePoint()
    {
        if(m_RandomizeDirectionTimer.IsFinished == true)
        {
            Vector2 newPoint = UnityEngine.Random.onUnitSphere * m_PatrolPoint.Radius + m_PatrolPoint.transform.position;
            m_MovePosition = newPoint;
            m_RandomizeDirectionTimer.Start(m_RandomSelectMovePointTime);
        }
    }

    public void SetPatrolBehaivour(AIPointPatrol point)
    {
        m_AIBehaivour = AIBehaivour.Patrol;
        m_PatrolPoint = point;
    }
    
    #endregion

}
