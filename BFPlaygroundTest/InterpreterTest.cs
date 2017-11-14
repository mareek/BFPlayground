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
            const string programWithMismatchingBracketNumber = "[[-]";
            Check.ThatCode(() => new Interpreter(programWithMismatchingBracketNumber)).ThrowsAny();

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
    }
}
