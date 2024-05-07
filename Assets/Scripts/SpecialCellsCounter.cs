using UnityEngine;

public class SpecialCellsCounter : MonoBehaviour
{
    public int threshold = 3; // The threshold to trigger options
    private int specialCellCount = 0;
    public GameModifier gameModifier;
    public SpecialCellProgressBar specialCellProgressBar;

    public void AddSpecialCell()
    {
        specialCellCount++;
    }
    public void UpdateCount()
    {   
        while (specialCellCount >= threshold)
        {
            gameModifier.ShowOptions(); // Trigger the options display
            specialCellCount -= threshold;
        }
        specialCellProgressBar.UpdateBar(specialCellCount);
    }
    public int CurrentSpecialCellCount => specialCellCount;
}
