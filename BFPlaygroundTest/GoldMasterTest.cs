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

                    if (lineNumber % 3 == 0)
                        title = line;
                    else if (lineNumber % 3 == 1)
                        program = line;
                    else
                        yield return new object[] { title, program, Convert.FromBase64String(line) };

                    lineNumber++;
                }
            }
        }

        [Theory]
        [MemberData(nameof(GetReferenceData))]
        public void TestGoldMasterCorpus(string title, string program, byte[] expectedBinaryOutput)
        {
            var interpreter = new Interpreter(program);
            var deadline = DateTime.Now.Add(TimeSpan.FromMilliseconds(500));
            while (!interpreter.EndOfProgram && DateTime.Now < deadline)
                interpreter.Step();

            Assert.True(interpreter.EndOfProgram, $"The program didn't finished execution in the allocated time : {title}");
            Check.That(interpreter.BinaryOutput).ContainsExactly(expectedBinaryOutput);
        }
    }
}
