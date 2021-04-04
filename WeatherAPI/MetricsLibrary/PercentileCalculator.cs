using System;
using System.Collections.Generic;

namespace MetricsLibrary
{
    public static class PercentileCalculator
    {
        public static int Calculate(List<int> values, double percentileValue)
        { 
        /*public static int Calculate<T>(List<T> metricValues, double percentileValue)
        {
            Type type = metricValues.GetType();

            foreach (Type interfaceType in type.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IList<>))
                {
                    Type itemType = type.GetGenericArguments()[0];

                     CpuMetricModel.
                    break;
                }
            }
            metricValues.

            if (metricValues.GetType() is IList<CpuMetric>)

            HashSet<int> set = new HashSet<int>();

            foreach (var metric in metricValues)
            {
                set.Add(metric.Value);
            }

            //return new List<int>(set);
        */
            if (values.Count == 0) return -1;

            int[] sequence = new List<int>(values).ToArray();

            Array.Sort(sequence);
            int N = sequence.Length;
            double n = (N - 1) * percentileValue + 1;

            if (n == 1d) return sequence[0];
            else if (n == N) return sequence[N - 1];
            else
            {
                int k = (int)n;
                double d = n - k;

                return Array.Find(sequence, p => p > (sequence[k - 1] + d * (sequence[k] - sequence[k - 1])));
            }
        }

        public static double Calculate(List<double> values, double percentileValue)
        {
            /*public static int Calculate<T>(List<T> metricValues, double percentileValue)
            {
                Type type = metricValues.GetType();

                foreach (Type interfaceType in type.GetInterfaces())
                {
                    if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IList<>))
                    {
                        Type itemType = type.GetGenericArguments()[0];

                         CpuMetricModel.
                        break;
                    }
                }
                metricValues.

                if (metricValues.GetType() is IList<CpuMetric>)

                HashSet<int> set = new HashSet<int>();

                foreach (var metric in metricValues)
                {
                    set.Add(metric.Value);
                }

                //return new List<int>(set);
            */
            if (values.Count == 0) return -1;

            double[] sequence = new List<double>(values).ToArray();

            Array.Sort(sequence);
            int N = sequence.Length;
            double n = (N - 1) * percentileValue + 1;

            if (n == 1d) return sequence[0];
            else if (n == N) return sequence[N - 1];
            else
            {
                int k = (int)n;
                double d = n - k;

                return Array.Find(sequence, p => p > (sequence[k - 1] + d * (sequence[k] - sequence[k - 1])));
            }
        }
    }
}
