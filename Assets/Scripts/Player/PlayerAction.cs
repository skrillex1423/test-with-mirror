using Mirror;
using UnityEngine;
public abstract class PlayerAction : NetworkBehaviour
{
    protected Player Player { get; private set; }

    private void OnEnable()
    {
        Player = GetComponent<Player>();
    }
}
