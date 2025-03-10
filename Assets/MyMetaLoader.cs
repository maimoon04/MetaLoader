using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MyMetaLoader", menuName = "ScriptableObject/MyMetaLoader")]

public class MyMetaLoader : ScriptableObject
{
    public TextAsset csvFile; // Assign via Inspector OR load via Resources
    public ArmoryContainer armoryContainer; // Assign in Inspector or create at runtime

    void Start()
    {
        LoadCSV();
    }

    [ContextMenu("loadData")]
   public  void LoadCSV()
    {
        armoryContainer.armoryObjects.Clear();

        if (csvFile == null)
        {
            csvFile = Resources.Load<TextAsset>("armory_data"); // filename without extension
        }

        if (csvFile == null)
        {
            Debug.LogError("CSV file not found!");
            return;
        }

        string[] lines = csvFile.text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        List<ArmoryObjects> objects = new List<ArmoryObjects>();

        for (int i = 1; i < lines.Length; i++) // Skip header
        {
            string[] parts = lines[i].Split(',');

            if (parts.Length < 4)
            {
                Debug.LogWarning($"Skipping invalid line {i + 1}");
                continue;
            }

            try
            {
                ArmoryObjects obj = new ArmoryObjects
                {
                    WeaponType = (WeaponsType)Enum.Parse(typeof(WeaponsType), parts[0].Trim()),
                    Currency = (Currency)Enum.Parse(typeof(Currency), parts[1].Trim()),
                    Amount = int.Parse(parts[2].Trim()),
                    path = parts[3].Trim()
                };

                objects.Add(obj);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error parsing line {i + 1}: {ex.Message}");
            }
        }

        if (armoryContainer == null)
        {
            armoryContainer = ScriptableObject.CreateInstance<ArmoryContainer>();
        }

        armoryContainer.armoryObjects = objects;

        Debug.Log($"Loaded {objects.Count} armory objects from CSV.");
    }
}
