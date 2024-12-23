﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing;

internal class Interval
{
    public double min;
    public double max;

    public Interval()
    {
        min = float.PositiveInfinity; 
        max = float.PositiveInfinity;
    }

    public Interval(double min, double max)
    {
        this.min = min;
        this.max = max;
    }

    public double Size()
    {
        return max - min;
    }
    public bool Contains(double x)
    {
        return min <= x && x <= max;
    }

    public bool Surrounds(double x)
    {
        return min < x && x <= max;
    }
}


internal static class IntervalConstants
{
    public static Interval Empty = new(float.PositiveInfinity, float.NegativeInfinity);
    public static Interval Universe = new(float.NegativeInfinity, float.PositiveInfinity);
}
