using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainFCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            BrainFCSharp.Class.Interpreter Interpreter = new Class.Interpreter();

            Interpreter.Program(",.", 30000);
            Interpreter.InputText('b');
            Interpreter.Finish();
            Console.ReadKey();
        }
    }
}
