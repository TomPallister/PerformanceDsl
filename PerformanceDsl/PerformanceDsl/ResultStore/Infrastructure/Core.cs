﻿using System;

namespace PerformanceDsl.ResultStore.Infrastructure
{
    public static class Core
    {
        /// <summary>
        ///     Returns DBNull if the object is null, suitable for DB param values
        /// </summary>
        public static object DetermineParamValue(object obj)
        {
            if (obj == null)
                return DBNull.Value;
            return obj;
        }
    }
}