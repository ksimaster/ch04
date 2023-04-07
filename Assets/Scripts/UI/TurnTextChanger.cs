using TMPro;
using UnityEngine;

public class TurnTextChanger : MonoBehaviour
{
    private TextMeshProUGUI turnText;
    private Animator textAnimator;
    private string color;

    private void Start()
    {
        turnText = GetComponent<TextMeshProUGUI>();
        textAnimator = GetComponent<Animator>();
    }

    public void ChangeTurnText(PawnColor pawnColor)
    {
        color = pawnColor.ToString().ToUpper() == "WHITE" ? "БЕЛЫХ" : "ЧЕРНЫХ";
        // turnText.text = pawnColor.ToString().ToUpper() + "'S TURN";
        turnText.text = "ХОД " + color;
        textAnimator.SetTrigger("NextTurn");
    }
}