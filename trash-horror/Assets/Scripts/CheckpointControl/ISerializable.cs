using System;
using System.Collections.Generic;

public interface ISerializable
{
    public Dictionary<String, String> Serialize();
    
    public void Deserialize(Dictionary<String, String> serialized);
}