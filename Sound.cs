public enum Sound
{ 
   Arrow = 0,
   ArrowHit = 1,
   Mageball = 2,
   MageballHit = 3,
   EnemyDie = 4,
   EnemyWin = 5,
   PlayerWin = 6,
   PlayerLose = 7,
   BGM = 8,
   BGMLevel = 9,
   EnemySpawn = 10,
   TowerBuilt = 11,
   Click = 12,
   FireAbilitySelected = 13,
   FireAbilityUsed = 14,
   TimeAbilityUsed = 15,
}
public static class SoundExtensions
{
    public static void Play(this Sound sound)
    {
        SoundPlayer.Instance.Play(sound);
    }
}
