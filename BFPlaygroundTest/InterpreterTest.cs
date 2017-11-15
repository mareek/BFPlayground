using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFPlayground;
using Xunit;
using NFluent;

namespace BFPlaygroundTest
{
    public class InterpreterTest
    {
        [Fact]
        public void IncorrectProgramsTest()
        {
            const string programWithUnclosedBracket = "[[-]";
            Check.ThatCode(() => new Interpreter(programWithUnclosedBracket)).ThrowsAny();

            const string programWithMismatchingBrackets = "]-[";
            Check.ThatCode(() => new Interpreter(programWithMismatchingBrackets)).ThrowsAny();
        }

        [Fact]
        public void Test42()
        {
            const string program = "[-]>[-]<>+++++++[<+++++++>-]<+++.--.";
            const string expectedOutput = "42";
            var interpreter = new Interpreter(program);
            interpreter.Run();
            Assert.Equal(expectedOutput, interpreter.Output);
            Check.That(interpreter.Data).HasSize(2);
            Check.That(interpreter.Data[0]).IsEqualTo(50);
            Check.That(interpreter.Data[1]).IsEqualTo(0);
            Check.That(interpreter.DataPointer).IsEqualTo(0);
        }

        [Fact]
        public void TestInnerLoop()
        {
            const string program =
@"[-]>[-]< initialise les 2 premières cellules mémoire à 0 (en cas de mémoire non initialisée)
>+++++++[<+++++++>-]< initialise la première cellule mémoire au caractère ASCII '1'
>>+++[<<.+>>-<+[-]>]";
            const string expectedOutput = "123";
            var interpreter = new Interpreter(program);
            interpreter.Run();
            Assert.Equal(expectedOutput, interpreter.Output);
        }

        [Fact]
        public void TestSkipInnerLoop()
        {
            const string program =
@"[-]>[-]< initialise les 2 premières cellules mémoire à 0 (en cas de mémoire non initialisée)
>+++++++[<+++++++>-]< initialise la première cellule mémoire au caractère ASCII '1'
.>[<.>[-]<.>]";
            const string expectedOutput = "1";
            var interpreter = new Interpreter(program);
            interpreter.Run();
            Assert.Equal(expectedOutput, interpreter.Output);
        }
    }
}
