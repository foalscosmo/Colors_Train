using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkinManager : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> trainSlots = new();
    [SerializeField] private PassengerSpawner passengerSpawner;
    [SerializeField] private BuildingSpawner buildingSpawner;
    [SerializeField] private FruitsSpawner fruitsSpawner;
    [SerializeField] private List<Color> colors = new();
    [SerializeField] private List<int> colorIndex = new();

    private void Awake()
    {
        ShuffleTogether(colors,colorIndex);
        SetColorToTrainSlots();
        SetSkinToBuildings();
    }

    private void Start()
    {
        SetSkinToPassengers();
        SetSkinToFruits();
    }

    private void SetColorToTrainSlots()
    {
        for (int i = 0; i < trainSlots.Count; i++)
        {
            trainSlots[i].color = colors[i];
            trainSlots[i].tag = colorIndex[i].ToString();
        }
    }

    private void SetSkinToPassengers()
    {
        ShuffleTogether(colors,colorIndex);
        
        for (int i = 0; i < trainSlots.Count; i++)
        {
            passengerSpawner.DraggedObjects[i].SpriteRenderer.color = colors[i];
            passengerSpawner.DraggedObjects[i].tag = colorIndex[i].ToString();
        }
    }

    private void SetSkinToBuildings()
    {
        ShuffleTogether(colors,colorIndex);
        
        for (int i = 0; i < trainSlots.Count; i++)
        {
            buildingSpawner.BuildingSpriteRenderers[i].color = colors[i];
            buildingSpawner.BuildingObjects[i].tag = colorIndex[i].ToString();
        }
    }

    private void SetSkinToFruits()
    {
        ShuffleTogether(colors,colorIndex);
        
        for (int i = 0; i < trainSlots.Count; i++)
        {
            fruitsSpawner.DraggedObjects[i].SpriteRenderer.color = colors[i];
            fruitsSpawner.DraggedObjects[i].tag = colorIndex[i].ToString();
        }
    }

    private void ShuffleTogether(IList<Color> list1, IList<int> list2)
    {
        int n = Mathf.Min(list1.Count, list2.Count); 
        
        for (var i = n - 1; i > 0; i--)
        {
            var j = Random.Range(0, i + 1);
            (list1[i], list1[j]) = (list1[j], list1[i]);
            (list2[i], list2[j]) = (list2[j], list2[i]);
        }
    }
}