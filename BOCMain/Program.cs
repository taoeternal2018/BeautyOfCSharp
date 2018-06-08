using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOCMain
{
    class Program
    {
        /// <summary>
        /// 委托在编译的时候确实会被编译成类（class）。 因为Delegate是一个类，所以在任何可以声明类的地方都可以声明委托。
        /// 委托是一个类，它定义了方法的类型，使得可以将方法当作另一个方法的参数来进行传递，这种将方法动态地赋给参数的做
        ///     法，可以避免在程序中大量使用If-Else(Switch)语句，同时使程序具有更好的可扩展性。
        /// 使用委托可以将多个方法绑定到同一个委托变量，当调用此变量时（这里用“调用”这个词，是因为此变量代表一个方法），
        ///     可以依次调用所有绑定的方法
        /// </summary>
        /// <param name="name"></param>
        public delegate void GreetingDelegate(string name);
        static void Main(string[] args)
        {
            TestGeneric();
            TestDelegate();
            Console.ReadKey();
        }

        static void TestGeneric() {
            BOCGeneric bg = new BOCGeneric();
            SomeComparable[] scs = new SomeComparable[100];
            bg.SpeedSort(scs);//这里用到一个有趣的编译器能力，那就是可以推断出传递的数组类型，以及是否满足了泛型约束
            bg.SpeedSort<SomeComparable>(scs);//这个写法可以简化成上面写法
        }

        static void TestDelegate() {
            GreetPeople("Joe", EnglishGreeting);
            GreetPeople("Liming", ChineseGreeting);
            GreetingDelegate gd = ChineseGreeting;
            gd += EnglishGreeting;
            GreetPeople("Bruce", gd);
            gd("Doris");
            //GreetPeople("Other", OtherGreeting);//wrong func signature 

            GreetingManager gm = new GreetingManager();
            gm.MakeGreet += EnglishGreeting;
            gm.MakeGreet += ChineseGreeting;
            gm.GreetPeople("JJJJ");
        }
        static void GreetPeople(string name, GreetingDelegate makeGreeting) {
            makeGreeting(name);
        }
        static void ChineseGreeting(string name) {
            Console.WriteLine("ChineseGreeting:"+name);
        }
        static void EnglishGreeting(string name) {
            Console.WriteLine("EnglishGreeting:"+name);
        }
        static string OtherGreeting(string name) {
            return "OtherGreeting"+name;
        }
    }
}
