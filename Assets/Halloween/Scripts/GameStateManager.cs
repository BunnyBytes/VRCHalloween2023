
using System.Collections.Generic;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

/// <summary>
/// This is a class to manage the game state so late joiners can sync properly
/// </summary>
public class GameStateManager : UdonSharpBehaviour
{
    [SerializeField] GameObject VRCWorld;
    public List<VRCPlayerApi> players = new List<VRCPlayerApi>();

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
            players.Add(player);
        }
    }

    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        if (Networking.IsOwner(gameObject))
        {
            Debug.Log($"Removing player {player.displayName} from players list");
            players.Remove(player);
        }
    }
}
