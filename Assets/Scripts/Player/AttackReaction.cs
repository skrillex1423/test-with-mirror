using System.Collections;
using UnityEngine;
using Mirror;

public class AttackReaction : PlayerAction
{
    [SerializeField] private Color _attackedColor = Color.red;
    [SerializeField] private float _effectDuration = 3f;
    [field: SerializeField] public bool AttackImmunity { get; private set; }

    [Command]
    void CmdChangeColor()
    {
        RpcChangeColor();
    }

    [ClientRpc]
    public void RpcChangeColor()
    {
        Player.Material.color = _attackedColor;
        StartCoroutine(WaitEffectDuration(_effectDuration));
        AttackImmunity = true;
    }

    private IEnumerator WaitEffectDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        Player.Material.color = Color.white;
        AttackImmunity = false;
    }
}
