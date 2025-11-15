using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public static class Vector3Serialization
{
    public static string Serialize(this Vector3 position) => 
        $"x: {position.x.ToString(CultureInfo.CurrentCulture)}," +
        $"y: {position.y.ToString(CultureInfo.CurrentCulture)}," +
        $"z: {position.z.ToString(CultureInfo.CurrentCulture)}";

    public static Vector3 Deserialize(string vector)
    {
        var temp = vector.Split(",").Select(pair => pair.Split(":")).ToArray();
        var x = temp[0][1];
        var y = temp[1][1];
        var z = temp[2][1];
        
        return new Vector3(
            float.Parse(x, CultureInfo.InvariantCulture),
            float.Parse(y, CultureInfo.InvariantCulture),
            float.Parse(z, CultureInfo.InvariantCulture));
    }
}