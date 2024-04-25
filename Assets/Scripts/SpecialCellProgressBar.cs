using UnityEngine;
using UnityEngine.UI;

public class SpecialCellProgressBar : MonoBehaviour
{
    public Slider progressBar;
    public SpecialCellsCounter specialCellsCounter;

    void Update()
    {
        if (specialCellsCounter != null && progressBar != null)
        {
            progressBar.value = specialCellsCounter.CurrentSpecialCellCount;
        }
    }
}
