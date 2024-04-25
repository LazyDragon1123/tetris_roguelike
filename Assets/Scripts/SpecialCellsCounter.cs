using UnityEngine;

public class SpecialCellsCounter : MonoBehaviour
{
    public int threshold = 10; // The threshold to trigger options
    private int specialCellCount = 0;
    public GameModifier gameModifier; // Reference to the GameModifier to call ShowOptions

    public void AddSpecialCell()
    {
        specialCellCount++;
        if (specialCellCount >= threshold)
        {
            gameModifier.ShowOptions(); // Trigger the options display
            specialCellCount = 0; // Reset the count or keep accumulating depending on the game design
        }
    }
    public int CurrentSpecialCellCount => specialCellCount;
}
