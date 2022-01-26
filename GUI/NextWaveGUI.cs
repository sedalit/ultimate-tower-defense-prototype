using UnityEngine;
using UnityEngine.UI;

public class NextWaveGUI : MonoBehaviour
{
    [SerializeField] private Text m_BonusAmount;
    [SerializeField] private GameObject m_Tip;
    [SerializeField] private Image m_ClockFill;

    private EnemyWaveManager waveManager;
    private float timeToNextWave;
    //-- События Юнити
    private void Awake()
    {
        waveManager = FindObjectOfType<EnemyWaveManager>();
        EnemyWave.OnWavePrepare += (float time) =>
        {
            timeToNextWave = time;
            if (m_ClockFill != null) m_ClockFill.fillAmount = timeToNextWave;
        };
    }
    private void Update()
    {
        if (waveManager.CurrentWave == null) gameObject.SetActive(false);

        var bonus = (int)timeToNextWave;
        if (waveManager.CurrentWave == waveManager.FisrtWave) bonus = 0;
            if (bonus < 0) bonus = 0;
        m_BonusAmount.text = bonus.ToString();
        timeToNextWave -= Time.deltaTime;
        m_ClockFill.fillAmount = timeToNextWave/10;
    }
    // -- //
    public void CallWave()
    {
        waveManager.ForceNextWave((int)waveManager.CurrentWave.GetRemainingTime());
    }
}
