using System;

public static class NumberExtensions {

    public static string GetRandomNumber(int count)
    {
        string number = string.Empty;

        for (int i = 0; i < count; i++)
        {
            number += UnityEngine.Random.Range(0, 9);
        }

        return number;
    }

    public static int ToInt(this object obj)
    {
        try
        {
            return Convert.ToInt32(obj);
        }
        catch
        {
            return 0;
        }
    }
    public static bool ToBool(this object obj)
    {
        return Convert.ToBoolean(obj);
    }
    public static bool IntToBool(this int obj)
    {
        if (obj == 0) return false;
        return true;
    }
    public static int BoolToInt(this bool obj)
    {
        if (obj == true) return 1;
        return 0;
    }

    public static double ToRadians(this double val)
    {
        return (Math.PI / 180) * val;
    }

    public static int FloorTo(this int value, int interval)
    {
        try
        {
            var remainder = value % interval;
            return value - remainder;
        }
        catch 
        {
            return 0;
        }
    }
}
