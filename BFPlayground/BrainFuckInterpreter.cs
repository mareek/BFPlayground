using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BFPlayground
{
    class BrainFuckInterpreter
    {
        private const string instructions = "<>+-[].,";

        private string _program;

        private char[] _code;
        private int _codePointer = 0;

        private List<byte> _data = new List<byte> { 0 };
        int _dataPointer = 0;

        List<byte> _output;

        int _lastOpeningBracket = -1;

        public BrainFuckInterpreter(string program)
        {
            _program = program;
            _code = _program.Where(c => instructions.Contains(c)).ToArray();
        }

        public string Run()
        {
            _output = new List<byte>();

            while (_codePointer < _code.Length)
            {
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

            return new string(UTF8Encoding.UTF8.GetChars(_output.ToArray()));
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
