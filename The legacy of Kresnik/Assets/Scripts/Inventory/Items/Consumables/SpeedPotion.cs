using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Speed potion", menuName = "Items/Speed Potion", order = 6)]
public class SpeedPotion : Item, IUseable
{
    [SerializeField]
    private int speed;

    [SerializeField]
    private int time;

    public void Use()
    {
            Remove();
            Player.MyInstance.StartCoroutine(Player.MyInstance.IncreaseSpeed(speed, time));
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n<color=#00ff00ff>Use: Increases the speed by +{0} for {1} </color>", speed, time);
    }
}
