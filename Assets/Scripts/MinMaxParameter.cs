using System;
using UnityEngine;

[Serializable]
public class MinMaxParameter
{
    public float minValue;
    public float maxValue;

    public MinMaxParameter()
    {
        minValue = maxValue = 0f;
    }

    public MinMaxParameter(float minValue, float maxValue)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
    }
}
