using BFPlayground;
using NFluent;
using Xunit;

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

        private const string ProgramHelloWorld =
@">++++++++[<+++++++++>-]<.>>+>+>++>[-]+<[>[->+<<++++>]<<]>.+++++++..+++.>
>+++++++.<<<[[-]<[-]>]<+++++++++++++++.>>.+++.------.--------.>>+.>++++.";

        [Theory]
        [InlineData("Hello world", ProgramHelloWorld, "Hello World!\n")]
        [InlineData("Inner loop", "[-]>[-]<>+++++++[<+++++++>-]<>>+++[<<.+>>-<+[-]>]", "123")]
        [InlineData("Skipped inner loop", "[-]>[-]<>+++++++[<+++++++>-]<.>[<.>[-]<.>]", "1")]
        public void TestProgramOutput(string title, string program, string expectedOutput)
        {
            Assert.NotEmpty(title);
            var interpreter = new Interpreter(program);
            interpreter.Run();
            Assert.Equal(expectedOutput, interpreter.Output);
        }
    }
}
