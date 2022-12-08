using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortionItem : CountableItem ,IUsableItem
{
    public PortionItem(PortionItemData data, int amount = 1) : base(data, amount) { }
    public PlayableCharacter Player;
    public bool Use()
    {
        Debug.Log("포션");
        Amount--;
        //if (Player == null)
        //    Player = PlayableCharacter.Instance;

        //Debug.Log(Player.name);

        //Player.status.HPUp(50);
        return true;
    }

    protected override CountableItem Clone(int amount)
    {
        return new PortionItem(CountableData as PortionItemData, amount);
    }

    public int E_Use()
    {
        return 1;
    }
}
