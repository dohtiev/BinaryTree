using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTrey
{
    public class CTimer
    {
        public static (string, T) Timer<T>(Func<T> function)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            T result = function();
            stopwatch.Stop();
            string elapsedTime = stopwatch.Elapsed.ToString();

            return (elapsedTime, result);
        }
        public static string Timer(Action function)
        {
            long temp = 0;
            const int iter = 1000;
            Stopwatch stopwatch;
            for (int i = 0; i < iter; i++)
            {
                stopwatch = new Stopwatch();
                stopwatch.Start();
                function();
                stopwatch.Stop();
                temp += stopwatch.ElapsedTicks;
            }

            string elapsedTime = TimeSpan.FromTicks(temp / iter).ToString(@"hh\:mm\:ss\.fffffff"); ;

            return elapsedTime;
        }
    }
    public enum TraversalType
    {
        Preorder,
        Inorder,
        Postorder
    }
}
