using UnityEngine;
using Mirror;
using System;

public class DashAction : PlayerAction
{
    [SerializeField] private string _tagAttack = "Player";
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _distance = 10;
    [SerializeField, SyncVar] private bool _dashing = false;
    [SerializeField, SyncVar] private int _hits = 0;
    private Vector3 _targetPosition;
    public bool Dashing => _dashing;

    public event Action<NetworkIdentity, int> OnHit;

    private void Update()
    {
        if (_dashing)
        {
            if (isLocalPlayer)
            {
                Player.transform.position = Vector3.MoveTowards(Player.transform.position, _targetPosition, _speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, _targetPosition) < 0.01f)
                    CmdEndDash();
            }
        }
    }

    [Command]
    public void CmdStartDash()
    {
        RpcStartDashAnimation();
    }

    [ClientRpc]
    public void RpcStartDashAnimation()
    {
        Player.Rigidbody.velocity = Vector3.zero;
        Player.Rigidbody.isKinematic = true;
        _targetPosition = Player.transform.position + Player.CharacterController.transform.forward * _distance;
        Player.SetCharacterAction(CharacterActionType.Dash);
        Player.Animator.SetTrigger("Dash");
        _dashing = true;
    }

    [Command]
    public void CmdEndDash()
    {
        Player.Rigidbody.isKinematic = false;
        _dashing = false;
        RpcStopDashAnimation();
    }

    [ClientRpc]
    public void RpcStopDashAnimation()
    {
        Player.Animator.SetTrigger("StopDash");
    }

    [ServerCallback]
    private void OnCollisionEnter(Collision collision)
    {
        if (isServer)
        {
            if (collision.gameObject.CompareTag(_tagAttack)
                && !collision.gameObject.Equals(Player.gameObject))
            {
                Player characterEnemy = collision.gameObject.GetComponent<Player>();
                if (!characterEnemy.AttackReaction.AttackImmunity && Player.DashAction.Dashing)
                {
                    characterEnemy.AttackReaction.RpcChangeColor();
                    OnHit?.Invoke(Player.netIdentity, _hits + 1);
                    RpcChangeScore();
                }
            }
        }
    }

    [ClientRpc]
    public void RpcChangeScore()
    {
        _hits++;
        Player.CanvasRotator.SetText(_hits.ToString());
    }

    [ClientRpc]
    public void RpcClear()
    {
        _dashing = false;
        _hits = 0;
        Player.CanvasRotator.SetText(_hits.ToString());
    }
}
