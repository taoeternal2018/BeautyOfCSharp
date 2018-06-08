using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOCMain
{
    /// <summary>
    /// 5.1.1 泛型和委托
    /// The difference between Func and Action is simply whether you want the delegate 
    /// to return a value (use Func) or not (use Action).
    /// 
    /// Predicate is just a special cased Func<T, bool> really, introduced before all 
    /// of the Func and most of the Action delegates came along. I suspect that if we'd
    /// already had Func and Action in their various guises, Predicate wouldn't have 
    /// been introduced... although it does impart a certain meaning to the use of the
    /// delegate, whereas Func and Action are used for widely disparate purposes.
    /// 
    /// 5.1.2 隐式类型的局部变量 var
    /// </summary>
    class TestINQ
    {
        public delegate void Doit(object sender, EventArgs e);
        public Doit JustDo;
        //5.1.3 匿名类型
        public void TestAnonymousClass() {
            //并没有定义developer变量的类型，它的类型由编译器进行推断和定义
            //匿名类型的属性的类型，例如Name、 Age的类型，也是由编译器推断和定义的
            var dev = new {Name="Jimmy", Age=30,Favorites = new[] { ".net","C#","jQuery"} };
            Console.WriteLine(dev.Name +dev.Age+string.Join(",",dev.Favorites));
            var s = "test";
            s.MakeString();
        }

        public void AnonymousVSLambda() {
            //5.1.5 匿名方法和Lambda表达式
            JustDo += delegate (object sender, EventArgs e)
            {
                Console.WriteLine("您好，我的读者，希望您能喜欢本书！");
            };
            JustDo += (x, y) => Console.WriteLine("....");

            //不使用花括号时，默认会将方法体的执行结果作为返回值；而使用了花括号，尽管
            //方法体的语句只有一条，仍然需要使用return明确地返回执行结果
            Predicate<int> pre = x => x == 3;
            Predicate<int> pre2 = x =>{ return x == 3; };
        }
    }
    //5.1.4 扩展方法:
    public static class Test {
        public static string MakeString(this string s) {
            return s + " done";
        }
    }

    //5.2 集合
    public class NewTest {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class ProductCollection : IEnumerable<NewTest>
    {
        public IEnumerator<NewTest> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
    //5.3 LINQ查询
    //LINQ是一个大家族，尽管底层的实现各不相同，但对应用程序来说调用方式是一致
    //的，这种一致性是通过一种松散的约束来保证的，即LINQ家族的成员都需要提供对其数据
    //源的通用操作，这些通用操作包括筛选、 连接、 排序、 投影、 分组等。 在LINQ中，这些操
    //作叫做LINQ运算符（LINQ operators）或LINQ运算符方法（LINQ operator methods）
    public class LINQQuery {
        ProductCollection col = new ProductCollection();
        void Test() {
            //查询表达式
            var query = from x in col
                        where x.Age > 18
                        orderby x.Name
                        select new { Description = x.Name, Age = x.Age + 10 };
            //链式调用
            var query2 = col.Where(x => x.Age > 18)
                        .OrderBy(x => x.Name)
                        .Select(x => new { Description = x.Name, Age = x.Age + 10 });

            //混合使用
            //并不是所有的扩展方法都有对应的查询表达式，并且大多数扩展方法都有多个重载，
            //某些重载方法不存在查询表达式。 但是像上面那样，将两种查询方式混用起来使用
            //可以较好地解决这个问题。 不过混合使用不应该太过随意，应该遵循一定的原则，
            //这样可以保持代码的一致性和可读性。 一般来说，有这样两种策略：
            //1）只使用链式方法调用。 但在个别情况下，比如两个集合的连接操作中，使用链式方
            //   法调用会使语句变得很复杂，此时应混合使用查询表达式。
            //2）只使用查询表达式。 但对于某些查询表达式不支持的操作，例如Concat()，混合使
            //   用方法调用
            var query3 = from x in col.Where(x => x.Age > 18)
                         orderby x.Name
                         select new { Description = x.Name, Age = x.Age + 10 };

            foreach (var item in query)
            {
                Console.WriteLine("{0}|{1}", item.Description, item.Age);
            }
        }
    }

    /// <summary>
    /// 5.4 LINQ查询运算符
    /// LINQ一共包含了超过50个查询运算符，根据返回值的不同分为四类
    /// 5.4.1 返回IEnumerable<T>
    ///     Where/OfType/Cast/OrderBy、ThenBy、OrderByDescending、ThenByDescending/Select/Take、Skip/TakeWhile、SkipWhile/Reverse/DefaultIfEmpty/Distinct/GroupBy/Intersect(相交交叉)、Except（第一个有第二个没有）/Concat、Union/Zip
    /// 5.4.2 返回其他序列类型
    ///     ToArray()、 ToList()、 ToDictionary()、 ToLookUp()
    /// 5.4.3 返回序列中元素
    ///     1.ElementAt()和ElementAtOrDefault()
    ///     2.First()和FirstOrDefault()
    ///     3.Last()和LastOrDefault()
    ///     4.Single()和SingleOrDefault()
    /// 5.4.4 返回标量值
    ///     1.Count()和LongCount()
    ///     2.Max()、 Min()、 Average()和Sum()
    ///     3.Aggregate()
    ///     4.Contains()、 Any()、 All()和SequenceEqual()
    /// 5.4.5 其他方法:还有一些方法，它们也定义在System.Linq.Enumerable类中，却不是扩展方法，只是普通的静态方法，因此，只能在Enumerable类上调用
    /// IEnumerable<string> list1 = Enumerable.Empty<string>(); //创建一个空的string序列
    /// IEnumerable<int> list2 = Enumerable.Empty<int>(); //创建一个空的int序列
    /// Console.WriteLine(list1.Count()); // 0
    /// 
    /// IEnumerable<int> seq = Enumerable.Range(0, 3); //0,1,2
    /// IEnumerable<int> seq = Enumerable.Repeat(0, 3); // 0,0,0
    /// </summary>
    class tmpp {
    }
}
