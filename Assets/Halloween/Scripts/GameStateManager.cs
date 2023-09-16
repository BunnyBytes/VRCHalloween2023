
using System.Collections.Generic;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;

/// <summary>
/// This is a class to manage the game state so late joiners can sync properly
/// </summary>
public class GameStateManager : UdonSharpBehaviour
{
    [SerializeField] GameObject VRCWorld;
    [SerializeField] public DataList playerIds = new DataList();

    // When this variable is updated, set the VRC world object's position to it for all users
    [UdonSynced, FieldChangeCallback(nameof(SpawnPoint))]
    private Vector3 _spawnPoint;
    public Vector3 SpawnPoint
    {
        set
        {
            // Update the synced spawn point
            Debug.Log($"Setting the spawn point to {value}");
            _spawnPoint = value;
            VRCWorld.transform.position = _spawnPoint;
            RequestSerialization();
        }
        get => _spawnPoint;
    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (Networking.IsOwner(gameObject))
        {
            Debug.Log($"Adding player {player.displayName} to players list");
            DataToken _player = player.playerId;
            playerIds.Add(_player.Int);
        }
    }

    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        if (Networking.IsOwner(gameObject))
        {
            Debug.Log($"Removing player {player.displayName} from players list");
            DataToken _player = player.playerId;
            playerIds.Remove(_player.Int);
        }
    }
}
