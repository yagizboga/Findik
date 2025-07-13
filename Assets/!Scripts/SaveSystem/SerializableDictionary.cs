using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<Tkey,Tvalue> : Dictionary<Tkey,Tvalue>,ISerializationCallbackReceiver
{
    [SerializeField] private List<Tkey> keys;
    [SerializeField] private List<Tvalue> values;
    public void OnBeforeSerialize(){
        keys.Clear();
        values.Clear();
        foreach(KeyValuePair<Tkey,Tvalue> pair in this){
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }
    public void OnAfterDeserialize(){
        this.Clear();

        if(keys.Count != values.Count){
            Debug.LogError("Keys and values count aren't the same!");
        }
        for(int i = 0;i<keys.Count;i++){
            this.Add(keys[i],values[i]);
        }
    }
}
