using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainFCSharp.Class
{
    /// <summary>
    /// The main interpreter for the program.
    /// You should be able to create multiple instinces of this that won't interfere with eachother.
    /// </summary>
    class Interpreter
    {
        private char[] ProgramData;
        private uint ArrayLength;
        private bool ProgramedBefore = false;

        /// <summary>
        /// Upload a program to execute to the instince.
        /// </summary>
        /// <param name="Program">The program, all in 1 string.</param>
        /// <param name="ArrayLength">The length of the byte array.</param>
        public void Program(string Program, uint ArrayLength)
        {
            if (ProgramedBefore == false)
            {
                ProgramData = Program.ToCharArray();
                this.ArrayLength = ArrayLength;
            }
            else
                throw new ArgumentException("The program has already been defined.");
        }

        private List<byte> Input = new List<byte>();

        /// <summary>
        /// Inputs 1 char into the program, can be used while the program is running.
        /// </summary>
        /// <param name="Charactor">A single letter to input to the program.</param>
        public void InputText(char Charactor)
        {
            Input.Add(Encoding.Default.GetBytes(Charactor.ToString())[0]);      // This converts the char to a byte in ascii codes. If you want to change it, your probably better off rewriting it.
        }

        /// <summary>
        /// Executes the program untill it is finished executing.
        /// </summary>
        public void Finish()
        {
            while (Progress != ProgramData.Length)
                OneStep();
        }

        private bool ExecutedBefore = false;

        private int Progress = 0;
        private byte[] ByteArray;
        private uint Pointer;

        /// <summary>
        /// Executes exactly 1 instruction. Usefull for setting the speed the program runs at.
        /// </summary>
        public void OneStep()
        {
            if (ExecutedBefore == false)
                ByteArray = new byte[ArrayLength];

            if (ProgramData[Progress] == '+')
                AddOne();

            else if (ProgramData[Progress] == '-')
                SubtractOne();

            else if (ProgramData[Progress] == '>')
                PointerLeft();

            else if (ProgramData[Progress] == '<')
                PointerRight();

            else if (ProgramData[Progress] == '.')
                OutputByte();

            else if (ProgramData[Progress] == ',')
                InputByte();

            else if (ProgramData[Progress] == '[')
                OpenBracket();

            else if (ProgramData[Progress] == ']')
                CloseBracket();

            Progress++;
        }

        // Starting the instruction set.

        /// <summary>
        /// Adds 1 to the cerrently selected byte.
        /// </summary>
        private void AddOne()
        {
            ByteArray[Pointer]++;
        }

        /// <summary>
        /// Subtracts 1 from the cerrently selected byte.
        /// </summary>
        private void SubtractOne()
        {
            ByteArray[Pointer]--;
        }

        /// <summary>
        /// Move the pointer left 1 byte.
        /// </summary>
        private void PointerLeft()
        {
            Pointer++;

            if (Pointer > ArrayLength)
                Pointer = 0;
        }

        /// <summary>
        /// Moves the pointer right 1 byte.
        /// </summary>
        private void PointerRight()
        {
            if (Pointer == 0)
                Pointer = ArrayLength;
            else
                Pointer--;      // Does array wraparound.
        }

        private char[] OutputingCharArray;
        private byte[] PointerPosAsByteArray = new byte[1];

        /// <summary>
        /// Outputs the cerrent byte to the console as an ascii char.
        /// </summary>
        private void OutputByte()
        {
            PointerPosAsByteArray[0] = ByteArray[Pointer];
            OutputingCharArray = System.Text.Encoding.ASCII.GetString(PointerPosAsByteArray).ToCharArray();     // Basically, converts the cerrently selected byte to a char.

            Console.Write(OutputingCharArray[0]);
        }

        private int InputProgress;

        /// <summary>
        /// Sets the byte at the pointer to the ascii code of the next input byte.
        /// </summary>
        private void InputByte()
        {
            ByteArray[Pointer] = Input[InputProgress];

            if (ByteArray[Pointer] != 0)
                InputProgress++;
        }

        private int NestLevel;

        /// <summary>
        /// If the cerrently selected byte is 0, than skip to the char after the next ]
        /// </summary>
        private void OpenBracket()
        {
            if (ByteArray[Pointer] == 0)
                while (true)
                {
                    Progress++;
                    if (ProgramData[Progress] == '[')
                        NestLevel++;

                    else if (ProgramData[Progress] == ']' && NestLevel == 0)       // Basically, if the cerrently selected byte is 0, than skip to the char after the next ]
                    {
                        Progress++;
                        break;
                    }

                    else if (ProgramData[Progress] == ']' && NestLevel != 0)
                        NestLevel--;
                }
        }

        /// <summary>
        /// Skips back to the last [ unless the byte at the pointer is 0.
        /// </summary>
        private void CloseBracket()
        {
            if (ByteArray[Pointer] != 0)
                while (true)
                {
                    Progress--;
                    if (ProgramData[Progress] == '[' && NestLevel == 0)       // Skips back to the last [ unless the byte at the pointer is 0.
                        break;

                    else if (ProgramData[Progress] == ']')
                        NestLevel++;

                    else if (ProgramData[Progress] == '[' && NestLevel != 0)
                        NestLevel--;
                }
        }
    }
}
