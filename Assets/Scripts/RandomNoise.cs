using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNoise
{
    private int seed;

    public RandomNoise(int seed) {
        this.seed = seed;
    }

    private int Hash(int seed) {
        /*
        int result = seed*0xEB32F1A;
        result = result << 3;
        result *= 0x83B2AF2;
        result = result << 3;
        result *= 0x83B2AF2;
        result += seed;
        result *= 0x15A8BB2F;
        return result;
        */
        //int limit = int.MaxValue;
        int x = seed;
        x *= x ^ 0x6b8f3a;
        //x &= limit;
        x *= seed;
        //x &= limit;
        //x += x^(x*2);
        x *= x ^ 0x9c24a0;
        //x &= limit;
        x *= x ^ 0xa415f0;
        //x &= limit;
        return x;
    }

    /**
     * Returns a random float between -1 and 1 for the given t.
     */
    public float Noise1D(float t) {
        int x = int.Parse(t.ToString().Replace(',','0').Replace('.','0'));
        return Hash(x)*1f/int.MaxValue;
    }

    /**
     * Returns a random float between -1 and 1 for the given x and y.
     */
    public float Noise2D(float x, float y)
    {
        int a = int.Parse(x.ToString().Replace(',', '0').Replace('.', '0'));
        int b = int.Parse(y.ToString().Replace(',', '0').Replace('.', '0'));
        return Hash(a+b) * 1f / int.MaxValue;
    }
}
