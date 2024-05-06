using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    public Slider progressBar;

    public void UpdateBar(int damageCount)
    {
        progressBar.value = damageCount;
    }
}
