using UnityEngine;

public class BossHpCounter : MonoBehaviour
{
    public int FullHp = 5;
    private int bossDamageCount = 0;
    private bool isBossDead = false;
    public BossHpBar bossHpBar;

    public void AddCount()
    {
        bossDamageCount++;
        if (bossDamageCount >= FullHp)
        {
            bossDamageCount = 0; // Reset the count or keep accumulating depending on the game design
            isBossDead = true;
        }
        bossHpBar.UpdateBar(bossDamageCount);
    }
    public int CurrentBossDamageCount => bossDamageCount;
    public bool IsBossDead => isBossDead;
}
