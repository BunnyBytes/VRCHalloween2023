
using System.Collections.Generic;
using UdonSharp;
using UnityEngine;

/// <summary>
/// This is a class to manage the game state so late joiners can sync properly
/// </summary>
public class GameStateManager : UdonSharpBehaviour
{
    [SerializeField] GameObject VRCWorld;

    [UdonSynced, FieldChangeCallback(nameof(SpawnPoint))]
    private Vector3 _spawnPoint;
    // TODO: Test with late joiners
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
}
