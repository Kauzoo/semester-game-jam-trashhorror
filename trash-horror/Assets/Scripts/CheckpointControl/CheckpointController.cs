using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckpointController : MonoBehaviour, IGameEventListener
{
    public GameEvent respawnEvent;
    public DictionaryVariable savedGameObjects;
    public List<GameObject> gameObjectsToSave;
    
    private bool _isActive;

    private void OnEnable()
    {
        respawnEvent.RegisterListener(this);
    }
    
    private void OnDisable()
    {
        respawnEvent.UnregisterListener(this);
        savedGameObjects.value.Clear();
        _isActive = false;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || _isActive) return;
        
        _isActive = true;

        foreach(var gameObj in gameObjectsToSave)
        {
            savedGameObjects.value.Add(gameObj.name, SerializeGameObject(gameObj));
        }
    }

    private static Dictionary<string, string> SerializeGameObject(GameObject gameObj)
    { 
        return gameObj.GetComponent<ISerializable>()?.Serialize();
    }

    private static void DeserializeGameObject(GameObject gameObj, Dictionary<string, string> values)
    {
        gameObj.GetComponent<ISerializable>()?.Deserialize(values);
    }

    public void OnEventRaised()
    {
        // TODO Animation ?
        foreach (var gameObj in gameObjectsToSave)
        {
            DeserializeGameObject(gameObj, savedGameObjects.value[gameObj.name]);
        }
    }
}