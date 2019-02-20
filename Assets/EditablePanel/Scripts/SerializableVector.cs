using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Since unity doesn't flag the Vector3 as serializable, we
/// need to create our own version. This one will automatically convert
/// between Vector3 and SerializableVector3
/// </summary>
/// 
[System.Serializable]
public struct SerializableVector3
{
    /// <summary>
    /// x component
    /// </summary>
    public float x;

    /// <summary>
    /// y component
    /// </summary>
    public float y;

    /// <summary>
    /// z component
    /// </summary>
    public float z;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="rX"></param>
    /// <param name="rY"></param>
    /// <param name="rZ"></param>
    public SerializableVector3(float rX, float rY, float rZ)
    {
        x = rX;
        y = rY;
        z = rZ;
    }

    /// <summary>
    /// Returns a string representation of the object
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return String.Format("[{0}, {1}, {2}]", x, y, z);
    }

    /// <summary>
    /// Automatic conversion from SerializableVector3 to Vector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Vector3(SerializableVector3 rValue)
    {
        return new Vector3(rValue.x, rValue.y, rValue.z);
    }

    /// <summary>
    /// Automatic conversion from Vector3 to SerializableVector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator SerializableVector3(Vector3 rValue)
    {
        return new SerializableVector3(rValue.x, rValue.y, rValue.z);
    }

    public static SerializableVector3[] FromVector3Array(Vector3[] rValue)
    {
        SerializableVector3[] temp = new SerializableVector3[rValue.Length];
        for (int index = 0; index < rValue.Length; ++index)
        {
            temp[index] = rValue[index];
        }
        return temp;
    }

    public static Vector3[] ToVector3Array(SerializableVector3[] rValue)
    {
        Vector3[] temp = new Vector3[rValue.Length];
        for (int index = 0; index < rValue.Length; ++index)
        {
            temp[index] = rValue[index];
        }
        return temp;
    }
}


[System.Serializable]
public struct SerializableVector2
{
    /// <summary>
    /// x component
    /// </summary>
    public float x;

    /// <summary>
    /// y component
    /// </summary>
    public float y;


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="rX"></param>
    /// <param name="rY"></param>
    public SerializableVector2(float rX, float rY)
    {
        x = rX;
        y = rY;
    }

    /// <summary>
    /// Returns a string representation of the object
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return String.Format("[{0}, {1}]", x, y);
    }

    /// <summary>
    /// Automatic conversion from SerializableVector2 to Vector2
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Vector2(SerializableVector2 rValue)
    {
        return new Vector2(rValue.x, rValue.y);
    }

    /// <summary>
    /// Automatic conversion from Vector2 to SerializableVector2
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator SerializableVector2(Vector2 rValue)
    {
        return new SerializableVector2(rValue.x, rValue.y);
    }

    public static SerializableVector2[] FromVector2Array(Vector2[] rValue)
    {
        SerializableVector2[] temp = new SerializableVector2[rValue.Length];
        for (int index = 0; index < rValue.Length; ++index)
        {
            temp[index] = rValue[index];
        }
        return temp;
    }

    public static Vector2[] ToVector2Array(SerializableVector2[] rValue)
    {
        Vector2[] temp = new Vector2[rValue.Length];
        for (int index = 0; index < rValue.Length; ++index)
        {
            temp[index] = rValue[index];
        }
        return temp;
    }
}
