using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

public class CombinationPuzzle : UdonSharpBehaviour
{
    int numberOfDigits = 4;

    [UdonSynced] string combinationSolution;
    [UdonSynced] string currentCombination;

    [SerializeField] Text text;
    [SerializeField] Text currentSolutionText;

    void Start()
    {
        if (Networking.IsOwner(Networking.LocalPlayer, gameObject))
        {
            // Set the random combination
            SetCombinations();

            RequestSerialization();
            text.text = combinationSolution;
        }
    }

    /// <summary>
    /// Generates a random combination lock of n digits
    /// </summary>
    /// <returns>Datalist representing ints corresponding to each digit of the combination lock</returns>
    void SetCombinations()
    {
        // Generate values for each digit as required
        for (int i = 0; i < numberOfDigits; i++)
        {
            combinationSolution += Random.Range(1, 10).ToString();
            currentCombination += "0";
        }
    }

    /// <summary>
    /// Given n digit and a direction, increases or decreases the desplay digit
    /// Called by the interactable object
    /// </summary>
    /// <param name="index"></param>
    /// <param name="increase"></param>
    public void ChangeCombinationDigit(int index, bool increase)
    {
        // Get the int at string index
        int currentDigit = int.Parse(currentCombination[index].ToString());

        // Increase or decrease digit
        if (increase)
        {
            currentDigit = (currentDigit + 1) % 10;
        }
        else
        {
            currentDigit = (currentDigit - 1 + 10) % 10;
        }

        // Update string
        char[] combinationArray = currentCombination.ToCharArray();
        combinationArray[index] = currentDigit.ToString()[0];
        currentCombination = new string(combinationArray);

        // Sync new value
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        RequestSerialization();
        currentSolutionText.text = currentCombination;
    }

    public override void OnDeserialization()
    {
        text.text = combinationSolution;
        currentSolutionText.text = currentCombination;
    }
}
