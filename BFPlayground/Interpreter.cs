﻿using System;
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

        private readonly List<byte> _output = new List<byte>();
        public byte[] BinaryOutput => _output.ToArray();
        public string Output => new string(Encoding.UTF8.GetChars(_output.ToArray()));

        private readonly int[] _jumpTable;

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
            _jumpTable = CreateJumpTable(_code);
        }

        private int[] CreateJumpTable(char[] code)
        {
            var jumpTable = new int[code.Length];
            var openingBrackets = new Stack<int>();
            for (int i = 0; i < code.Length; i++)
            {
                var instruction = code[i];
                if (instruction == '[')
                {
                    openingBrackets.Push(i);
                }
                else if (instruction == ']')
                {
                    var matchingBracket = openingBrackets.Pop();
                    jumpTable[i] = matchingBracket;
                    jumpTable[matchingBracket] = i;
                }
            }

            return jumpTable;
        }

        private static void CheckProgram(string program)
        {
            var unclosedLoopCount = 0;
            foreach (var instruction in program)
            {
                if (instruction == '[')
                    unclosedLoopCount++;
                else if (instruction == ']')
                    unclosedLoopCount--;

                if (unclosedLoopCount < 0)
                {
                    throw new Exception("Unexpected closing bracket");
                }
            }

            if (unclosedLoopCount > 0)
            {
                throw new Exception("Unclosed bracket");
            }
        }

        public void Run()
        {
            while (!EndOfProgram)
                Step();
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
            if (_data[DataPointer] == 0)
                _codePointer = _jumpTable[_codePointer];
        }

        private void ProcessClosingBracket()
        {
            if (_data[DataPointer] != 0)
                _codePointer = _jumpTable[_codePointer];
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
