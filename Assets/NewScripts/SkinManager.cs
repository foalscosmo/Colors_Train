using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public enum LayerEnum
{
    Color0,Color1,Color2
}

public class SkinManager : MonoBehaviour
{
    // [SerializeField] private List<SpriteRenderer> trainSlots = new();
    // [SerializeField] private PassengerSpawner passengerSpawner;
    // [SerializeField] private BuildingSpawner buildingSpawner;
    // [SerializeField] private FruitsSpawner fruitsSpawner;
    // [SerializeField] private List<Color> colors = new();
    // [SerializeField] private List<int> colorIndex = new();
    //
    // [SerializeField] private List<MatchingLayerType> trainSlotsLayer = new();
    // [SerializeField] private List<MatchingLayerType> buildingSlotsLayer = new();
    //
    // [SerializeField] private List<Sprite> buildingSprites = new();
    // [SerializeField] private List<Sprite> passengerSprites = new();
    // [SerializeField] private List<Sprite> fruitSprites = new();
    //
    // [SerializeField] private List<Sprite> trainSprites = new();
    //
    // private void Awake()
    // {
    //     ShuffleTogether(colors,colorIndex);
    //     SetColorToTrainSlots();
    //     SetSkinToBuildings();
    // }
    //
    // private void Start()
    // {
    //     SetSkinToPassengers();
    //     SetSkinToFruits();
    // }
    //
    // private void SetColorToTrainSlots()
    // {
    //     for (int i = 0; i < trainSlots.Count; i++)
    //     {
    //         trainSlots[i].sprite = trainSprites[i];
    //         trainSlotsLayer[i].MatchingLayer = (LayerEnum)colorIndex[i];
    //
    //     }
    // }
    //
    // private void SetSkinToPassengers()
    // {
    //     ShuffleTogether(colors,colorIndex);
    //     
    //     for (int i = 0; i < trainSlots.Count; i++)
    //     {
    //         passengerSpawner.DraggedObjects[i].SpriteRenderer.color = colors[i];
    //         passengerSpawner.DraggedObjects[i].DraggedLayer = (LayerEnum)colorIndex[i];
    //     }
    // }
    //
    // private void SetSkinToBuildings()
    // {
    //     // ShuffleTogether(colors,colorIndex);
    //     
    //     for (int i = 0; i < trainSlots.Count; i++)
    //     {
    //         buildingSpawner.BuildingSpriteRenderers[i].color = colors[i];
    //         buildingSlotsLayer[i].MatchingLayer = (LayerEnum)colorIndex[i];
    //     }
    // }
    //
    // private void SetSkinToFruits()
    // {
    //     //ShuffleTogether(colors,colorIndex);
    //     
    //     for (int i = 0; i < trainSlots.Count; i++)
    //     {
    //         fruitsSpawner.DraggedObjects[i].SpriteRenderer.color = colors[i];
    //         fruitsSpawner.DraggedObjects[i].DraggedLayer = (LayerEnum)colorIndex[i];
    //
    //     }
    // }
    //
    // private void ShuffleTogether(IList<Color> list1, IList<int> list2)
    // {
    //     int n = Mathf.Min(list1.Count, list2.Count); 
    //     
    //     for (var i = n - 1; i > 0; i--)
    //     {
    //         var j = Random.Range(0, i + 1);
    //         (list1[i], list1[j]) = (list1[j], list1[i]);
    //         (list2[i], list2[j]) = (list2[j], list2[i]);
    //
    //     }
    // }
    
    
    [SerializeField] private List<SpriteRenderer> trainSlots = new();
    [SerializeField] private List<SpriteRenderer> trainFrontSlots = new();
    [SerializeField] private PassengerSpawner passengerSpawner;
    [SerializeField] private BuildingSpawner buildingSpawner;
    [SerializeField] private FruitsSpawner fruitsSpawner;

    [SerializeField] private List<int> colorIndex = new(); // Defines which color each item should get
    [SerializeField] private List<MatchingLayerType> trainSlotsLayer = new();
    [SerializeField] private List<MatchingLayerType> buildingSlotsLayer = new();

    // These lists hold BLUE, PURPLE, YELLOW sprites in the same order
    [SerializeField] private List<Sprite> trainSprites = new(); 
    [SerializeField] private List<Sprite> buildingSprites = new();
    [SerializeField] private List<Sprite> passengerSprites = new();
    [SerializeField] private List<Sprite> fruitSprites = new();
    [SerializeField] private List<Sprite> trainFrontSprites = new();

    private void Awake()
    {
        ShuffleColors(); // Shuffle the color layers first
        ApplySkins(); // Apply correct sprites based on shuffled layers
    }

    private void ShuffleColors()
    {
        // Shuffle the color layers so objects get random color
        for (var i = colorIndex.Count - 1; i > 0; i--)
        {
            var j = Random.Range(0, i + 1);
            (colorIndex[i], colorIndex[j]) = (colorIndex[j], colorIndex[i]);
        }
    }

    private void ApplySkins()
    {
        for (int i = 0; i < trainSlots.Count; i++)
        {
            int layer = colorIndex[i]; // Get the shuffled color index

            // Assign correct train back sprite
            trainSlots[i].sprite = trainSprites[layer];

            // Assign correct train front sprite (same as train back)
            trainFrontSlots[i].sprite = trainFrontSprites[layer];

            // Set matching layer for train
            trainSlotsLayer[i].MatchingLayer = (LayerEnum)layer;

            // Assign correct building sprite
            buildingSpawner.BuildingSpriteRenderers[i].sprite = buildingSprites[layer];
            buildingSlotsLayer[i].MatchingLayer = (LayerEnum)layer;

            // Assign correct passenger sprite
            passengerSpawner.DraggedObjects[i].SpriteRenderer.sprite = passengerSprites[layer];
            passengerSpawner.DraggedObjects[i].DraggedLayer = (LayerEnum)layer;

            // Assign correct fruit sprite
            fruitsSpawner.DraggedObjects[i].SpriteRenderer.sprite = fruitSprites[layer];
            fruitsSpawner.DraggedObjects[i].DraggedLayer = (LayerEnum)layer;
        }
    }
}