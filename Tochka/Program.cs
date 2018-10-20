
using System;
using System.Text;

namespace Tochka
{
    public class Program
    {

        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Listener lst = new Listener();
            lst.Start();
        }        
    }
}
