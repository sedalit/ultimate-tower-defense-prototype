using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class Abilities : SingletoneBase<Abilities>
{
    [Serializable]
    public class ExplosionAbility
    {
        public GameObject costPanel;
        public Button useButton;
        public Text costText;
        [SerializeField] private UpgradeAsset m_ExplosionAbilityUpgrade;
        public UpgradeAsset GetAbilityUpgrade { get { return m_ExplosionAbilityUpgrade; } }
        [SerializeField] private Image targetingImage;
        [SerializeField] private int cost = 5;
        public int Cost => cost;
        [SerializeField] private int damage = 3;
        public void Use() 
        {
            Sound.FireAbilitySelected.Play();
            TDPlayer.Instance.ChangeMana(cost);
            ClickProtection.Instance.Activate((Vector2 v) =>
            {
                Vector3 position = v;
                position.z = -Camera.main.transform.position.z;
                position = Camera.main.ScreenToWorldPoint(position);
                foreach (var collider in Physics2D.OverlapCircleAll(position, 1f))
                {
                    if (collider.transform.parent.TryGetComponent<Enemy>(out var enemy))
                    {
                        Sound.FireAbilityUsed.Play();
                        enemy.TakeDamage(damage, TDProjectile.DamageType.Magic);
                    }
                }
            });
        }
    }

    [Serializable]
    public class SlowAbility
    {
        public GameObject costPanel;
        public Text costText;
        public Button useButton;
        [SerializeField] private UpgradeAsset m_SlowAbilityUpgrade;
        public UpgradeAsset GetAbilityUpgrade { get { return m_SlowAbilityUpgrade; } }
        [SerializeField] private float cooldown = 15f;
        [SerializeField] private int cost = 10;
        public int Cost => cost;
        [SerializeField] private float duration = 5f;
        public void Use()
        {
            Sound.TimeAbilityUsed.Play();
            TDPlayer.Instance.ChangeMana(cost);
            void Slow(Enemy enemy)
            {
                enemy.GetComponent<SpaceShip>().HalfMaxLinearVelocity();
            }
            foreach (var ship in FindObjectsOfType<SpaceShip>())
            {
                ship.HalfMaxLinearVelocity();
            }
            EnemyWaveManager.OnEnemySpawn += Slow;

            IEnumerator Restore()
            {
                yield return new WaitForSeconds(duration);
                foreach (var ship in FindObjectsOfType<SpaceShip>())
                {
                    ship.RestoreMaxLinearVelocity();
                }
                EnemyWaveManager.OnEnemySpawn -= Slow;
            }
            Instance.StartCoroutine(Restore());

            IEnumerator SlowAbilityButton()
            {
                useButton.interactable = false;
                yield return new WaitForSeconds(cooldown);
                useButton.interactable = true;
            }
            Instance.StartCoroutine(SlowAbilityButton());
        }
    }
    private void Start()
    {
        m_SlowUpgradeLevel = Upgrades.GetUpgradeLevel(m_SlowAbility.GetAbilityUpgrade);
        m_ExplosionUpgradeLevel = Upgrades.GetUpgradeLevel(m_ExplosionAbility.GetAbilityUpgrade);
        if (m_SlowUpgradeLevel >= 1)
        {
            m_SlowAbility.useButton.gameObject.SetActive(true);
            m_SlowAbility.costPanel.SetActive(true);
            m_SlowAbility.costText.text = m_SlowAbility.Cost.ToString();
        }
        if (m_ExplosionUpgradeLevel >= 1)
        {
            m_ExplosionAbility.useButton.gameObject.SetActive(true);
            m_ExplosionAbility.costPanel.SetActive(true);
            m_ExplosionAbility.costText.text = m_ExplosionAbility.Cost.ToString();
        }
        if (m_ExplosionUpgradeLevel == 0 && m_SlowUpgradeLevel == 0) m_ManaText.text = "X";
    }
    private void Update()
    {
        if (m_ExplosionUpgradeLevel >= 1 || m_SlowUpgradeLevel >= 1)
        {
            m_CurrentMana = TDPlayer.Instance.Mana;
            m_ManaText.text = m_CurrentMana.ToString();
            m_SlowAbility.useButton.interactable = m_CurrentMana >= m_SlowAbility.Cost;
            m_ExplosionAbility.useButton.interactable = m_CurrentMana >= m_ExplosionAbility.Cost;
        }
    }
    [SerializeField] private Text m_ManaText;
    [SerializeField] private ExplosionAbility m_ExplosionAbility;
    public void UseExplosionAbility() => m_ExplosionAbility.Use();
    [SerializeField] private SlowAbility m_SlowAbility;
    public void UseSlowAbility() => m_SlowAbility.Use();
    private int m_CurrentMana;
    private int m_SlowUpgradeLevel;
    private int m_ExplosionUpgradeLevel;
}
