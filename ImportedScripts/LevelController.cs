using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ILevelCondition
    {
        bool IsCompleted { get; }
    }
    
public class LevelController : SingletoneBase<LevelController>
{
    

    [SerializeField] protected float m_ReferenceTime;
    public float ReferenceTime => m_ReferenceTime;

    [SerializeField] protected UnityEvent m_EventOnLevelComplete;

    private ILevelCondition[] m_Conditions;
    private bool m_IsLevelCompleted;
    private float m_LevelTime;
    public float LevelTime => m_LevelTime;

    protected void Start() {
        m_Conditions = GetComponentsInChildren<ILevelCondition>();
    }

    private void Update() {
        if(!m_IsLevelCompleted)
        {
            m_LevelTime += Time.deltaTime;
            CheckLevelConditions();
        }
    }

    private void CheckLevelConditions()
    {
        if(m_Conditions == null || m_Conditions.Length == 0) return;

        int numCompleted = 0;

        foreach(var v in m_Conditions)
        {
            if(v.IsCompleted)
            {
                numCompleted++;
            }
        }

        if(numCompleted == m_Conditions.Length)
        {
            m_IsLevelCompleted = true;
            m_EventOnLevelComplete.Invoke();
            LevelSequenceController.Instance?.FinishCurrentLevel(true);
        }

    }
}
