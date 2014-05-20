using System;

namespace PerformanceDsl
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PerformanceTest : Attribute
    {
    }
}