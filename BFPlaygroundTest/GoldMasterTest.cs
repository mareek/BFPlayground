using System;
using System.Collections.Generic;
using System.IO;
using BFPlayground;
using NFluent;
using Xunit;

namespace BFPlaygroundTest
{
    public class GoldMasterTest
    {
        private static readonly TimeSpan ProgramMaxDuration = TimeSpan.FromMilliseconds(500);
        public static IEnumerable<object[]> GetReferenceData()
        {
            using (var fileStream = File.OpenText("Resources\\program corpus.txt"))
            {
                string title = "";
                string program = "";

                int lineNumber = 0;
                while (!fileStream.EndOfStream)
                {
                    var line = fileStream.ReadLine();

                    switch (lineNumber % 3)
                    {
                        case 0:
                            title = line;
                            break;
                        case 1:
                            program = line;
                            break;
                        case 2:
                            yield return new object[] { title, program, Convert.FromBase64String(line) };
                            break;
                    }

                    lineNumber++;
                }
            }
        }

        [Theory]
        [MemberData(nameof(GetReferenceData))]
        public void TestGoldMasterCorpus(string title, string program, byte[] binaryOutput)
        {
            var interpreter = new Interpreter(program);
            var deadline = DateTime.Now.Add(ProgramMaxDuration);
            while (!interpreter.EndOfProgram && DateTime.Now < deadline)
                interpreter.Step();

            Assert.True(interpreter.EndOfProgram, $"The program didn't finished execution in the allocated time : {title}");
            Check.That(interpreter.BinaryOutput).ContainsExactly(binaryOutput);
        }
    }
}
