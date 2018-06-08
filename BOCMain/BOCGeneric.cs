using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOCMain
{
    public class BOCGeneric
    {
        public void SpeedSort<T>(T[] array) where T : IComparable {
            Console.WriteLine(typeof(T).ToString());
        }
    }

    public class SomeComparable : IComparable
    {
        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
