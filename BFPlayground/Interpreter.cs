using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BFPlayground
{
    class Interpreter
    {
        private const string instructions = "<>+-[].,";

        private string _program;
        public string Program { get { return _program; } }
        public int ProgramPointer { get { return _translationTable[_codePointer]; } }

        public bool EndOfProgram { get { return _codePointer >= _code.Length; } }

        private char[] _code = null;
        private int[] _translationTable = null;
        private int _codePointer = 0;

        private List<byte> _data = new List<byte> { 0 };
        public ReadOnlyCollection<byte> Data { get { return _data.AsReadOnly(); } }

        int _dataPointer = 0;
        public int DataPointer { get { return _dataPointer; } }

        List<byte> _output;
        public string Output { get { return new string(UTF8Encoding.UTF8.GetChars(_output.ToArray())); } }

        int _lastOpeningBracket = -1;

        public Interpreter(string program)
        {
            _program = program;
        }

        private void Init()
        {
            if (_code == null)
            {
                var tempCode = new List<char>();
                var tempTranslationTable = new List<int>();
                for (var i = 0; i < _program.Length; i++)
                {
                    if (instructions.Contains(_program[i]))
                    {
                        tempCode.Add(_program[i]);
                        tempTranslationTable.Add(i);
                    }
                }
                _code = tempCode.ToArray();
                _translationTable = tempTranslationTable.ToArray();
                _output = new List<byte>();
            }
        }

        public string Run()
        {
            Init();
            while (_codePointer < _code.Length)
                Step();

            return this.Output;
        }

        public void Step()
        {
            Init();

            if (_codePointer >= _code.Length)
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
            if (_dataPointer == _data.Count - 1)
                _data.Add(0);
            _dataPointer++;
        }

        private void MoveCursorLeft()
        {
            if (_dataPointer == 0)
                throw new ApplicationException("Moved pointer before the beggining of the data");
            else
                _dataPointer--;
        }

        private void Increment()
        {
            _data[_dataPointer] += 1;
        }

        private void Decrement()
        {
            _data[_dataPointer] -= 1;
        }

        private void ProcessOpeningBracket()
        {
            _lastOpeningBracket = _codePointer;
            if (_data[_dataPointer] == 0)
            {
                while (_codePointer < _code.Length && _code[_codePointer] != ']')
                    _codePointer++;

                if (_codePointer == _code.Length)
                    throw new ApplicationException("No matching closing bracket");
            }
        }

        private void ProcessClosingBracket()
        {
            if (_data[_dataPointer] == 0)
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
            _output.Add(_data[_dataPointer]);
        }
    }
}
