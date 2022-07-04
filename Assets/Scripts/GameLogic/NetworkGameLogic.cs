using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Collections;
using UnityEngine.UI;

public class NetworkGameLogic : NetworkBehaviour
{
    private Dictionary<uint, Player> _players = new Dictionary<uint, Player>(10);
    [SerializeField] private GameObject _menu;
    [SerializeField] private int _hitsWin = 1;
    [SerializeField] private float _waitTimeToNextMatch = 5f;
    [SerializeField] private Text _menuText;
    public void AddNewPlayer(NetworkIdentity networkIdentity)
    {
        if (!isServer)
            return;

        Player player = networkIdentity.gameObject.GetComponent<Player>();
        player.DashAction.OnHit += CheckScore;

        _players.Add(networkIdentity.netId, player);
    }

    public void RemoveNewPlayer(uint id)
    {
        if (_players.ContainsKey(id))
        {
            _players.Remove(id);
        }
    }

    [Server]
    public void CheckScore(NetworkIdentity networkIdentity, int score)
    {
        if (isServer)
        {
            if (score >= _hitsWin)
            {
                RpcGameWin(networkIdentity);
                StartCoroutine(WaitTimeToStartNextMatch(_waitTimeToNextMatch));
            }
        }
    }

    [ClientRpc]
    private void RpcGameWin(NetworkIdentity networkIdentity)
    {
        _menuText.text = "Player " + networkIdentity.netId;
        _menu.gameObject.SetActive(true);
    }

    private void StartNextMatchServer()
    {
        var spawnPoints = NetworkManager.startPositions;
        int index = 0;
        foreach (var player in _players.Values)
        {
            var currentSpawnPoint = spawnPoints[index];
            player.RpcSetPosition(currentSpawnPoint.position);
            index++;
        }
    }

    [ClientRpc]
    private void StartNextMatchClient()
    {
        foreach (var player in _players.Values)
        {
            player.DashAction.RpcClear();
        }
        _menu.gameObject.SetActive(false);
    }

    private IEnumerator WaitTimeToStartNextMatch(float duration)
    {
        yield return new WaitForSeconds(duration);
        StartNextMatchServer();
        StartNextMatchClient();
    }
}
