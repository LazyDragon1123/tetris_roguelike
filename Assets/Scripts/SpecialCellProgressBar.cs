using UnityEngine;
using UnityEngine.UI;

public class SpecialCellProgressBar : MonoBehaviour
{
    public Slider progressBar;

    public void UpdateBar(int cellCount)
    {
        progressBar.value = cellCount;
    }
}
