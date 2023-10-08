
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UdonSharp;
using Unity.Mathematics;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

/// <summary>
/// This is a class to manage the game state so late joiners can sync properly
/// </summary>

public enum GameState
{
    lobby,
    inProgress
};
public class GameStateManager : UdonSharpBehaviour
{
    [SerializeField] int playerCapacity;

    [SerializeField] GameObject VRCWorld;

    GameState CurrentGameState = GameState.lobby;

    [SerializeField] GameObject[] TeleportPositions;

    [UdonSynced] Vector3 TeleportTarget;

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
            RequestSerialization();
        }
        get => _spawnPoint;
    }

    DataList CurrentPlayerIDs
    {
        set
        {
            CurrentPlayerIDsJSON = UdonSharpHelper.SerialiseToJson(value);
            RequestSerialization();
        }
        get => UdonSharpHelper.DeserialiseDatalist(CurrentPlayerIDsJSON);
    }

    [UdonSynced] string CurrentPlayerIDsJSON;


    /// <summary>
    /// Returns a DataList of ints representing the player ids of all the players currently in the instance
    /// </summary>
    /// <returns>DataList of players</returns>
    public DataList GetAllPlayerIds()
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

    /*
    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (Networking.IsOwner(Networking.LocalPlayer, gameObject))
        {
            Debug.Log($"Adding player {player.displayName} to the list of active players");
            activePlayers.Add(new DataToken(player.playerId));
            Debug.Log($"There are now {activePlayers.Count} players");
        }
    }

    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        if (Networking.IsOwner(Networking.LocalPlayer, gameObject))
        {
            DataToken playerToken = new DataToken(player.playerId);
            if (activePlayers.Contains(playerToken))
            {
                Debug.Log($"Removing player {player.displayName} to the list of active players");
                activePlayers.Remove(playerToken);
                Debug.Log($"There are now {activePlayers.Count} players");
            }
            else
            {
                Debug.LogError("Unable to find player when attempting to remove from active player list.");
            }
        }
    }
    */

    /// <summary>
    /// Start the game when the host clicks on the corresponding UI element
    /// </summary>
    public void StartGame()
    {
        Debug.Log("Button pressed");
        if (Networking.IsOwner(Networking.LocalPlayer, gameObject))
        {
            Debug.Log("Starting game!");
            CurrentGameState = GameState.inProgress;
            CurrentPlayerIDs = GetAllPlayerIds();

            TeleportTarget = TeleportPositions[0].transform.position;
            RequestSerialization();

            SendCustomNetworkEvent(NetworkEventTarget.All, "TeleportPlayersInSession");
        }
    }

    /// <summary>
    /// If the local player exists in the list of player ids representing the current session, teleport them to the teleport target
    /// </summary>
    public void TeleportPlayersInSession()
    {
        DataToken localId = new DataToken((double)Networking.LocalPlayer.playerId);
        if (CurrentPlayerIDs.Contains(localId))
        {
            Debug.Log($"Found local player id of {localId} in list, teleporting local player to {TeleportTarget}");
            Networking.LocalPlayer.TeleportTo(TeleportTarget, Quaternion.identity);
        }
        else
        {
            Debug.Log($"Did not send player with id of {localId} as they were not present in the current player list");
        }
    }
}
