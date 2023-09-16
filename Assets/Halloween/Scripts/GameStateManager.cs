
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
    [SerializeField] int playerCapacity;

    [SerializeField] GameObject VRCWorld;

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

    /// <summary>
    /// Returns a DataList of ints representing the player ids of all the players currently in the instance
    /// </summary>
    /// <returns></returns>
    public DataList GetAllPlayers()
    {
        VRCPlayerApi[] players = new VRCPlayerApi[playerCapacity];
        VRCPlayerApi.GetPlayers(players);

        // Convert the array to a DataList
        DataList playerIds = new DataList();
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null)
            {
                playerIds.Add(players[i].playerId);
            }
            else
            {
                break;
            }
        }

        // Return the DataList of integers
        return playerIds;
    }
}
