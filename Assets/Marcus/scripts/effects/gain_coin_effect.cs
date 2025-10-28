using UnityEngine;
[CreateAssetMenu(fileName = "gain_coin_effect", menuName = "ScriptableObjects/Skilleffects/gain_coin_effect", order = 1)]
public class gain_coin_effect : skilleffect
{
    public int coinAmount;

    public override void activeeffect(entity user, entity target, skilldata skilldata = null)
    {
        PlayerLineMove pm = user as PlayerLineMove;
        if (user is PlayerLineMove)
        {
            pm.current_coins += coinAmount;
            pm.ui.coin_text.text = pm.current_coins.ToString();
            Debug.Log($"P{pm.playerNumber} gained {coinAmount} coins. Current coins: {pm.current_coins}");
        }
    }

}
