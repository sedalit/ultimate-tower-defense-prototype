using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : SingletoneBase<Player>
{
    [SerializeField] private int m_NumLives;
    public int NumLives { get { return m_NumLives; } set { m_NumLives = value; } }
    public event Action OnPlayerDeath;

    [SerializeField] private SpaceShip m_Ship;
    public SpaceShip Ship => m_Ship;
    [SerializeField] private SpaceShip m_PlayerShipPrefab;

    //[SerializeField] private CameraController m_CameraController;
    //[SerializeField] private MovementController m_MovementController;
    
    [SerializeField] private float m_TimerBeforeRespawn = 5.0f;
    [SerializeField] private Text m_TimerText;
    [SerializeField] private GameObject m_ShowTimer;
    private bool m_IsDestroyed;

    protected override void Awake()
    {
        base.Awake();

        if(m_Ship != null)
        {
            Destroy(m_Ship.gameObject);
        }
    }

    private void Start() {
        if(m_Ship != null)
        {
        Respawn();
        m_Ship.EventOnDeath.AddListener(OnShipDeath);
        m_ShowTimer.SetActive(false);
        }
        
    }

    private void Update() {
        if(m_IsDestroyed == true)
        {
            m_TimerBeforeRespawn -= Time.deltaTime;
            m_TimerText.text = Mathf.Round(m_TimerBeforeRespawn).ToString();

            if(m_TimerBeforeRespawn < 0)
            {
                m_TimerBeforeRespawn = 0;
                Respawn();
            }    
        }
        
    }

    private void OnShipDeath()
    {
        m_NumLives--;
        if(m_NumLives > 0)
        {
            m_IsDestroyed = true;
            ShowTimer();
        }
        else
        {
            LevelSequenceController.Instance.FinishCurrentLevel(false);
        }
        

        //Respawn();
    }

    private void Respawn()
    {
        if(LevelSequenceController.PlayerShip != null)
        {
        m_ShowTimer.SetActive(false);
        m_IsDestroyed = false;
        var NewPlayerShip = Instantiate(LevelSequenceController.PlayerShip);

        m_Ship = NewPlayerShip.GetComponent<SpaceShip>();
	    Start();

        //m_CameraController.SetTarget(m_Ship.transform);
        //m_MovementController.SetTargetShip(m_Ship);
        }
    }

    private void ShowTimer()
    {
        m_ShowTimer.SetActive(true);
    }

    public int Score { get; private set; }
    public int NumKills { get; private set; }

    public void AddKill()
    {
        NumKills++;
    }

    public void AddScore(int num)
    {
        Score += num;
    }

    protected void TakeDamage(int m_Damage)
    {
        m_NumLives -= m_Damage;
        if(m_NumLives <= 0)
        {
            m_NumLives = 0;
            OnPlayerDeath?.Invoke();
        }
    }
}
