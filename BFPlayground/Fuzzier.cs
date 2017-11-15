using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFPlayground
{
    class Fuzzier
    {
        public string GenerateProgramWithOuput()
        {
            long tryCount = 0;
            const string allowedInstructions = "<>+-[].";
            const int maxLength = 500;
            var maxProgramDuration = TimeSpan.FromMilliseconds(100);
            var rng = new Random();

            char GetRandomInstruction() => allowedInstructions[rng.Next(allowedInstructions.Length)];
            var programLength = rng.Next(maxLength);

            string program;
            do
            {
                tryCount++;
                var programBuilder = new StringBuilder();
                var unclosedLoopCount = 0;
                for (int i = 0; i < programLength; i++)
                {
                    char instruction;

                    do
                    {
                        instruction = GetRandomInstruction();
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

                program = programBuilder.ToString();
            } while (!IsProgramExecutableInDefinedTimespan(program, maxProgramDuration, out var output)
                    || string.IsNullOrEmpty(output));

            return program;
        }

        private bool IsProgramExecutableInDefinedTimespan(string program, TimeSpan maxDuration, out string output)
        {
            try
            {
                var interpreter = new Interpreter(program);
                var deadLine = DateTime.Now.Add(maxDuration);
                while (!interpreter.EndOfProgram && DateTime.Now < deadLine)
                {
                    interpreter.Step();
                }

                output = interpreter.Output;

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
