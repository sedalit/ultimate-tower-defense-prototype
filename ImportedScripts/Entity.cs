using UnityEngine;
/// <summary>
/// Базовый класс всех интерактивных объектов на сцене.
///</summary>

public abstract class Entity : MonoBehaviour
{
    /// <summary>
    /// Название объекта для пользователя.
    /// </summary>
    [SerializeField]
    protected string m_Nickname;
    public string Nickname => m_Nickname;
}
