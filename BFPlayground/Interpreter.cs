using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BFPlayground
{
    public class Interpreter
    {
        private const string instructions = "<>+-[].,";
        public string Program { get; }
        public int ProgramPointer => _translationTable[_codePointer];

        public bool EndOfProgram => _codePointer >= _code.Length;

        private char[] _code = null;
        private int[] _translationTable = null;
        private int _codePointer = 0;

        private List<byte> _data = new List<byte> { 0 };
        public ReadOnlyCollection<byte> Data => _data.AsReadOnly();

        public int DataPointer { get; private set; } = 0;

        List<byte> _output;
        public string Output => new string(Encoding.UTF8.GetChars(_output.ToArray()));

        int _lastOpeningBracket = -1;

        public Interpreter(string program)
        {
            CheckProgram(program);

            Program = program;
            var tempCode = new List<char>();
            var tempTranslationTable = new List<int>();
            for (var i = 0; i < Program.Length; i++)
            {
                if (instructions.Contains(Program[i]))
                {
                    tempCode.Add(Program[i]);
                    tempTranslationTable.Add(i);
                }
            }
            _code = tempCode.ToArray();
            _translationTable = tempTranslationTable.ToArray();
            _output = new List<byte>();
        }

        private void CheckProgram(string program)
        {
            var openingBracketCount = 0;
            foreach (var instruction in program)
            {
                switch (instruction)
                {
                    case '[':
                        openingBracketCount++;
                        break;
                    case ']':
                        openingBracketCount--;
                        break;
                }

                if (openingBracketCount < 0)
                {
                    throw new Exception("Unexpected closing bracket");
                }
            }

            if (openingBracketCount > 0)
            {
                throw new Exception("Unclosed bracket");
            }
        }

        public string Run()
        {
            while (!EndOfProgram)
                Step();

            return Output;
        }

        public void Step()
        {
            if (EndOfProgram)
                throw new ApplicationException("End of program reached");

            switch (_code[_codePointer])
            {
                case '>':
                    MoveCursorRight();
                    break;
                case '<':
                    MoveCursorLeft();
                    break;
                case '+':
                    Increment();
                    break;
                case '-':
                    Decrement();
                    break;
                case '[':
                    ProcessOpeningBracket();
                    break;
                case ']':
                    ProcessClosingBracket();
                    break;
                case ',':
                    GetInput();
                    break;
                case '.':
                    PrintOutput();
                    break;
            }
            _codePointer++;
        }

        private void MoveCursorRight()
        {
            if (DataPointer == _data.Count - 1)
                _data.Add(0);
            DataPointer++;
        }

        private void MoveCursorLeft()
        {
            if (DataPointer == 0)
                throw new ApplicationException("Moved pointer before the beggining of the data");
            else
                DataPointer--;
        }

        private void Increment()
        {
            _data[DataPointer] += 1;
        }

        private void Decrement()
        {
            _data[DataPointer] -= 1;
        }

        private void ProcessOpeningBracket()
        {
            _lastOpeningBracket = _codePointer;
            if (_data[DataPointer] == 0)
            {
                while (_codePointer < _code.Length && _code[_codePointer] != ']')
                    _codePointer++;

                if (_codePointer == _code.Length)
                    throw new ApplicationException("No matching closing bracket");
            }
        }

        private void ProcessClosingBracket()
        {
            if (_data[DataPointer] == 0)
            { /*Do nothing*/ }
            else if (_lastOpeningBracket >= 0)
                _codePointer = _lastOpeningBracket;
            else
                throw new ApplicationException("No matching opening bracket");
        }

        private void GetInput()
        {
            //TODO
        }

        private void PrintOutput()
        {
            _output.Add(_data[DataPointer]);
        }
    }
}
