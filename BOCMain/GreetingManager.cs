using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOCMain
{
    delegate void GreetingDelegate(string name);
    class GreetingManager
    {
        /// <summary>
        /// event封装了委托类型的变量，相当于为委托类型量身定制的属性,使得：在类的内部，
        /// 不管声明它是public还是protected，它总是private的。在类的外部，注册“+=”和注
        /// 销“-=”的访问限定符与声明事件时使用的访问限定符相同
        /// 
        /// 声明一个事件不过类似于声明一个进行了封装的委托类型的变量而已
        /// 
        /// 使用事件不仅能获得比委托更好的封装性，还能限制含有事件的类型的能力。 这是什么
        /// 意思呢？它的意思是说：事件应该由事件的发布者触发，而不应该由事件的客户端（客户程
        /// 序）来触发
        /// </summary>
        public event GreetingDelegate MakeGreet;
        public void GreetPeople(string name)
        {
            MakeGreet(name);
            MakeGreet = null;

            var delegateList = MakeGreet?.GetInvocationList();
            //foreach (var d in delegateList)//need to check if delegateList is null or not(complier is not smart enough)
            foreach (var d in delegateList!=null?delegateList:new GreetingDelegate[0]) {
                d.ToString();
            }

            if (delegateList != null) {
                var delArray = MakeGreet.GetInvocationList();
                foreach (var del in delArray) {
                    try
                    {
                        //EventHandler method = (EventHandler)del;
                        //method(this, EventArgs.Empty);
                        del.DynamicInvoke(this, EventArgs.Empty);//a more commonly function, applies to all types of delegate
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

        }
        public void UseDelegateVSEvent()
        {
            Publishser pub = new Publishser();
            Subscriber sub = new Subscriber();
            pub.NumberChanged += new NumberChangedEventHandler(sub.OnNumberChanged);
            pub.DoSomething(); // 应该通过DoSomething()来触发事件
            pub.NumberChanged(100); // 也可以被这样直接调用，对委托变量的
        }
    }

    public delegate void NumberChangedEventHandler(int count);
    // 定义事件发布者
    public class Publishser
    {
        private int count;
        public NumberChangedEventHandler NumberChanged; // 声明委托变量
        /// <summary>
        /// 通过添加event关键字来发布事件，事件发布者的封装性会更好，事件仅仅供其他
        /// 类型订阅，而客户端不能直接触发事件（语句pub.NumberChanged(100)无法通过
        /// 编译），事件只能在事件发布者Publisher类的内部触发
        /// </summary>
        //public event NumberChangedEventHandler NumberChanged; // 声明一个事件
        public void DoSomething()
        {
            // 在这里完成一些工作 ...
            if (NumberChanged != null)
            { // 触发事件
                count++;
                NumberChanged(count);
            }
        }
    }
    // 定义事件订阅者
    public class Subscriber
    {
        public void OnNumberChanged(int count)
        {
            Console.WriteLine("Subscriber notified: count = {0}", count);
        } 
    }
}