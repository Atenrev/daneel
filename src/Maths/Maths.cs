using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maths
{
    public static float Sigmoid(float input)
    {
        return 1 / (1 + Mathf.Exp(-input));
    }

    public static float RandomNormal()
    {
        System.Random rand = new System.Random(); 
        double mean = 0, stdDev = 1;
        //uniform(0,1] random doubles
        double u1 = 1.0 - rand.NextDouble(); 
        double u2 = 1.0 - rand.NextDouble();
        //random normal(0,1)
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                     Math.Sin(2.0 * Math.PI * u2);
        //random normal(mean,stdDev^2)
        double randNormal =
                     mean + stdDev * randStdNormal; 
        return (float) randNormal;
    }
}
