using System;
using System.Text;
using System.Text.RegularExpressions;

namespace CPUemu
{
    public static class CommandsHandler
    {
        public static void Randomize()
        {
            Random random = new Random();
            Reg.AH = (byte)random.Next(256);
            Reg.BH = (byte)random.Next(256);
            Reg.CH = (byte)random.Next(256);
            Reg.DH = (byte)random.Next(256);
            Reg.AL = (byte)random.Next(256);
            Reg.BL = (byte)random.Next(256);
            Reg.CL = (byte)random.Next(256);
            Reg.DL = (byte)random.Next(256);
            CommandsHandler.ViewReg();
        }

        public static void Mov(string destination, string source)
        {
            SetValue(destination, GetValue(source));
            ViewReg();
        }

        public static void Xchg(string destination, string source)
        {
            byte destValue = GetValue(destination);
            byte srcValue = GetValue(source);

            SetValue(destination, srcValue);
            SetValue(source, destValue);

            ViewReg();
        }

        public static void ViewReg()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"AH: {Reg.AH:X2}\tAL: {Reg.AL:X2}");
            stringBuilder.AppendLine($"BH: {Reg.BH:X2}\tBL: {Reg.BL:X2}");
            stringBuilder.AppendLine($"CH: {Reg.CH:X2}\tCL: {Reg.CL:X2}");
            stringBuilder.AppendLine($"DH: {Reg.DH:X2}\tDL: {Reg.DL:X2}");
            Console.WriteLine(stringBuilder);
        }

        public static void ViewWelcomeMessage()
        {
            Console.WriteLine("Welcome to CPU emulator!");
            Console.WriteLine("Enter /help to view available commands.");
            ViewReg();
        }

        public static void Help()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("/mov <destination>, <source>");
            Console.WriteLine("/xchg <destination>, <source>");
            Console.WriteLine("/add <destination>, <source>");
            Console.WriteLine("/sub <destination>, <source>");
            Console.WriteLine("/inc <destination>");
            Console.WriteLine("/dec <destination>");
            Console.WriteLine("/not <destination>");
            Console.WriteLine("/and <destination>");
            Console.WriteLine("/or <destination>");
            Console.WriteLine("/xor <destination>");
            Console.WriteLine("/randomize - insert random values into registers");
            Console.WriteLine("/reg - view registers");
            Console.WriteLine("/exit - exit the application");
        }

        public static void Not(string destination)
        {
            byte value = GetValue(destination);
            value = (byte)~((int)value);
            SetValue(destination, value);
            ViewReg();
        }

        public static void Add(string destination, string source)
        {
            byte destinationValue = GetValue(destination);
            byte sourceValue = GetValue(source);
            if (sourceValue == byte.MinValue)
            {
                if (Regex.IsMatch(source, @"^(0[xX])?[0-9a-fA-F]+$"))
                {
                    try
                    {
                        sourceValue = Convert.ToByte(source, 16);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Invalid hexadecimal format");
                        return;
                    }
                    catch (OverflowException)
                    {
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid register");
                    return;
                }
            }
            try
            {
                checked
                {
                    byte result = (byte)(destinationValue + sourceValue);
                    SetValue(destination, result);
                }
            }
            catch (OverflowException)
            {
                Console.WriteLine("Overflow occurred");
            }
            ViewReg();
        }

        public static void Sub(string destination, string source)
        {
            byte destinationValue = GetValue(destination);
            byte sourceValue = GetValue(source);
            if (sourceValue == byte.MinValue)
            {
                if (Regex.IsMatch(source, @"^(0[xX])?[0-9a-fA-F]+$"))
                {
                    try
                    {
                        sourceValue = Convert.ToByte(source, 16);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Invalid hexadecimal format");
                        return;
                    }
                    catch (OverflowException)
                    {
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid register");
                    return;
                }
            }
            try
            {
                checked
                {
                    byte result = (byte)(destinationValue - sourceValue);
                    SetValue(destination, result);
                }
            }
            catch (OverflowException)
            {
                Console.WriteLine("Underflow occurred");
            }

            ViewReg();
        }

        public static void And(string destination, string source)
        {
            byte destValue = GetValue(destination);
            byte srcValue = GetValue(source);

            bool[] destBits = GetBits(destValue);
            bool[] srcBits = GetBits(srcValue);

            for (int i = 0; i < 8; i++)
            {
                destBits[i] &= srcBits[i];
            }

            byte result = GetByte(destBits);

            SetValue(destination, result);

            ViewReg();
        }

        public static void Or(string destination, string source)
        {
            byte destValue = GetValue(destination);
            byte srcValue = GetValue(source);

            byte[] destArray = BitConverter.GetBytes(destValue);
            byte[] srcArray = BitConverter.GetBytes(srcValue);

            for (int i = 0; i < destArray.Length; i++)
            {
                destArray[i] = (byte)(destArray[i] | srcArray[i]);
            }

            SetValue(destination, destArray[0]);

            ViewReg();
        }

        public static void Xor(string destination, string source)
        {
            byte destValue = GetValue(destination);
            byte srcValue = GetValue(source);

            byte[] destArray = BitConverter.GetBytes(destValue);
            byte[] srcArray = BitConverter.GetBytes(srcValue);

            for (int i = 0; i < destArray.Length; i++)
            {
                destArray[i] = (byte)(destArray[i] ^ srcArray[i]);
            }

            SetValue(destination, destArray[0]);

            ViewReg();
        }

        public static void Inc(string reg)
        {
            byte value = GetValue(reg);
            if (value == 255)
            {
                Console.WriteLine("Overflow occurred");
            }
            else
            {
                SetValue(reg, ++value);
            }
            ViewReg();
        }

        public static void Dec(string reg)
        {
            byte value = GetValue(reg);
            if (value == 0)
            {
                Console.WriteLine("Underflow occurred");
            }
            else
            {
                SetValue(reg, --value);
            }
            ViewReg();
        }

        private static void SetValue(string destination, byte value)
        {
            switch (destination)
            {
                case "AH":
                    Reg.AH = value;
                    break;
                case "BH":
                    Reg.BH = value;
                    break;
                case "CH":
                    Reg.CH = value;
                    break;
                case "DH":
                    Reg.DH = value;
                    break;
                case "AL":
                    Reg.AL = value;
                    break;
                case "BL":
                    Reg.BL = value;
                    break;
                case "CL":
                    Reg.CL = value;
                    break;
                case "DL":
                    Reg.DL = value;
                    break;
                default:
                    Console.WriteLine("Invalid register");
                    break;
            }
        }

        private static byte GetValue(string source)
        {
            switch (source)
            {
                case "AH":
                    return Reg.AH;
                case "BH":
                    return Reg.BH;
                case "CH":
                    return Reg.CH;
                case "DH":
                    return Reg.DH;
                case "AL":
                    return Reg.AL;
                case "BL":
                    return Reg.BL;
                case "CL":
                    return Reg.CL;
                case "DL":
                    return Reg.DL;
                default:
                    if (Regex.IsMatch(source, @"^(0[xX])?[0-9a-fA-F]+$"))
                    {
                        try
                        {
                            return Convert.ToByte(source, 16);
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Invalid hexadecimal format");
                            return byte.MinValue;
                        }
                        catch (OverflowException)
                        {
                            Console.WriteLine("Value out of range (0x00-0xFF)");
                            return byte.MinValue;
                        }
                    }
                    else
                    {
                        return byte.MinValue;
                    }
            }
        }

        private static bool[] GetBits(byte b)
        {
            bool[] bits = new bool[8];
            for (int i = 0; i < 8; i++)
            {
                bits[i] = (b & (1 << i)) != 0;
            }
            return bits;
        }

        private static byte GetByte(bool[] bits)
        {
            byte b = 0;
            for (int i = 0; i < 8; i++)
            {
                if (bits[i])
                {
                    b |= (byte)(1 << i);
                }
            }
            return b;
        }

    }
}
