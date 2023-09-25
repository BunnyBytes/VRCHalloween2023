
using JetBrains.Annotations;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class CombinationPuzzle : UdonSharpBehaviour
{
    int numberOfDigits = 4;
    DataList combination;
    [UdonSynced] string combinationJson;

    [SerializeField] Text text;

    // TODO: Remove combination lock debug message on publish
    void Start()
    {
        if (Networking.IsOwner(Networking.LocalPlayer, gameObject))
        {
            // Set the random combination
            combination = SetCombination();
            
            // Log combination and request serialisation
            Debug.Log($"Combination lock set to {GetCombinationString()}");
            RequestSerialization();
            text.text = GetCombinationString();
        }
    }

    /// <summary>
    /// Generates a random combination lock of n digits
    /// </summary>
    /// <returns>Datalist representing ints corresponding to each digit of the combination lock</returns>
    DataList SetCombination()
    {
        DataList combination = new DataList();

        // Generate random values for each digit as required
        for (int i = 0; i < numberOfDigits; i++)
        {
            combination.Add(new DataToken(Random.Range(0, 10)));
        }

        return combination;
    }

    public override void OnPreSerialization()
    {
        if (VRCJson.TrySerializeToJson(combination, JsonExportType.Minify, out DataToken result))
        {
            combinationJson = result.String;
        } 
        else
        {
            Debug.LogError(result.ToString());
        }
    }

    public override void OnDeserialization()
    {
        if (VRCJson.TryDeserializeFromJson(combinationJson, out DataToken result))
        {
            // The combination is returned as a DataList of Doubles
            combination = result.DataList;
        }
        else
        {
            Debug.LogError(result.ToString());
        }
        text.text = GetCombinationString();
    }

    /// <summary>
    /// Convert the data list to a string
    /// </summary>
    /// <returns>String combination</returns>
    string GetCombinationString()
    {
        string combinationLockString = "";
        for (int i = 0; i < combination.Count; i++)
        {
            if (combination.TryGetValue(i, out var value))
            {
                combinationLockString += value.ToString();
            }
        }

        return combinationLockString;
    }
}
