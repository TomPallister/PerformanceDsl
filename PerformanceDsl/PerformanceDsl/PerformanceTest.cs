using System;
using log4net.Core;

namespace PerformanceDsl
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PerformanceTest : Attribute
    {
    }
}