using UnityEngine;
using UnityEngine.UI;
using FishNet;

public class GameView : View
{
    public Dropdown cardNameDropDown;
    public Button endTurnButton;

    public void Update(){
        endTurnButton.interactable = (Player.Instance.sack.Count >= 2);
    }

    public override void Initialize(){
        cardNameDropDown.onValueChanged.AddListener((delegate {
                DropdownValueChanged(cardNameDropDown);
        }));
        base.Initialize();
    }

    void DropdownValueChanged(Dropdown change)
    {
        Player.Instance.setCardName(change.options[change.value].text);
    }
}
