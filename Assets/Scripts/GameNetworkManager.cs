using Mirror;
using System;
using System.Linq;
using UnityEngine;

public class GameNetworkManager : NetworkManager
{
    [SerializeField] private NetworkGameLogic _networkGameLogic;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        var player = conn.clientOwnedObjects.First();
        _networkGameLogic.AddNewPlayer(player);
    }

    [Obsolete]
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        _networkGameLogic.RemoveNewPlayer(conn.identity.netId);
    }
}