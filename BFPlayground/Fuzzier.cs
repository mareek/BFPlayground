using System;
using System.Linq;
using System.Text;

namespace BFPlayground
{
    class Fuzzier
    {
        private readonly Random rng = new Random();

        public string GenerateProgramWithOuput()
        {
            long tryCount = 0;
            const string weightedAllowedInstructions = "++++++---->>>>>>>>>>>>><<<<<<<<<<[]..";
            const int maxLength = 500;
            var maxProgramDuration = TimeSpan.FromMilliseconds(100);

            var programLength = rng.Next(maxLength);

            string program;
            do
            {
                tryCount++;
                program = GenerateProgram(weightedAllowedInstructions.ToCharArray(), programLength);
            } while (!IsProgramExecutableInDefinedTimespan(program, maxProgramDuration, out var output)
                    || !output.Any());

            return program;
        }

        private string GenerateProgram(char[] instructionPool, int programLength)
        {
            var programBuilder = new StringBuilder();
            var unclosedLoopCount = 0;
            for (int i = 0; i < programLength; i++)
            {
                char instruction;

                do
                {
                    instruction = instructionPool[rng.Next(instructionPool.Length)];
                } while (instruction == ']' && unclosedLoopCount == 0);

                if (instruction == '[')
                    unclosedLoopCount++;
                else if (instruction == ']')
                    unclosedLoopCount--;

                programBuilder.Append(instruction);
            }

            while (unclosedLoopCount > 0)
            {
                programBuilder.Append(']');
                unclosedLoopCount--;
            }

            return programBuilder.ToString();
        }

        private bool IsProgramExecutableInDefinedTimespan(string program, TimeSpan maxDuration, out byte[] output)
        {
            try
            {
                var interpreter = new Interpreter(program);
                var deadLine = DateTime.Now.Add(maxDuration);
                while (!interpreter.EndOfProgram && DateTime.Now < deadLine)
                {
                    interpreter.Step();
                }

                output = interpreter.BinaryOutput;

                return interpreter.EndOfProgram;
            }
            catch
            {
                output = null;
                return false;
            }
        }
    }
}
