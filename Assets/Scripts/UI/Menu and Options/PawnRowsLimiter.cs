using UnityEngine;
using UnityEngine.UI;

public class PawnRowsLimiter : MonoBehaviour
{
    private Slider pawnRowsSlider;

    private void Awake()
    {
        pawnRowsSlider = GetComponent<Slider>();
    }

    public void LimitMaxPawnRows(Slider BoardSizesSlider)
    {
        int boardSize = Mathf.RoundToInt(BoardSizesSlider.value);
        var val = (boardSize - 1) / 2;

        if (val > 3) val = 3;

        pawnRowsSlider.maxValue = val;
        if (pawnRowsSlider.value > pawnRowsSlider.maxValue)
            pawnRowsSlider.value = pawnRowsSlider.maxValue;
    }
}