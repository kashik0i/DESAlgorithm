using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace DESAlgorithm
{
    //REF:https://page.math.tu-berlin.de/~kant/teaching/hess/krypto-ws2006/des.htm
    class Program
    {
        /*
         * Step 1: Create 16 subkeys, each of which is 48-bits long.
         * The 64-bit key is permuted according to the following table,
         * PC-1. Since the first entry in the table is "57",
         * this means that the 57th bit of the original key K becomes the first bit of the permuted key K+.
         * The 49th bit of the original key becomes the second bit of the permuted key.
         * The 4th bit of the original key is the last bit of the permuted key.
         * Note only 56 bits of the original key appear in the permuted key.
         */
        static byte[][][] SBOX = new byte[8][][] {
            new byte[4][]{
            new byte[16]{14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7 },
            new byte[16]{0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8 },
            new byte[16]{4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0 },
            new byte[16]{ 15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13}
            },new byte[4][]{
            new byte[16]{15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10},
            new byte[16]{3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5},
            new byte[16]{0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15},
            new byte[16]{13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9}
            },new byte[4][]{
            new byte[16]{10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8},
            new byte[16]{13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1},
            new byte[16]{13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7},
            new byte[16]{1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12}
            },new byte[4][]{
            new byte[16]{7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15},
            new byte[16]{13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9},
            new byte[16]{10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4},
            new byte[16]{3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14}
            },new byte[4][]{
            new byte[16]{2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9},
            new byte[16]{14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6},
            new byte[16]{4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14},
            new byte[16]{11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3}
            },new byte[4][]{
            new byte[16]{12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11},
            new byte[16]{10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8},
            new byte[16]{9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6},
            new byte[16]{4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13}
            },new byte[4][]{
            new byte[16]{4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1},
            new byte[16]{13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6},
            new byte[16]{1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2},
            new byte[16]{6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12}
            },new byte[4][]{
            new byte[16]{13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7},
            new byte[16]{1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2},
            new byte[16]{7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8},
            new byte[16]{2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11}
            }};
        static byte[] IP = {
            58,50,42,34,26,18,10,2,
            60,52,44,36,28,20,12,4,
            62,54,46,38,30,22,14,6,
            64,56,48,40,32,24,16,8,
            57,49,41,33,25,17,9,1,
            59,51,43,35,27,19,11,3,
            61,53,45,37,29,21,13,5,
            63,55,47,39,31,23,15,7
        };
        static byte[] IPReverse = {
            40,8,48,16,56,24,64,32,
            39,7,47,15,55,23,63,31,
            38,6,46,14,54,22,62,30,
            37,5,45,13,53,21,61,29,
            36,4,44,12,52,20,60,28,
            35,3,43,11,51,19,59,27,
            34,2,42,10,50,18,58,26,
            33,1,41,9,49,17,57,25
        };
        static byte[] PC1 = {
            57,49,41,33,25,17,9,
            1,58,50,42,34,26,18,
            10,2,59,51,43,35,27,
            19,11,3,60,52,44,36,
            63,55,47,39,31,23,15,
            7,62,54,46,38,30,22,
            14,6,61,53,45,37,29,
            21,13,5,28,20,12,4
        };
        static byte[] PC2 = {
            14,17,11,24,1,5,
            3,28,15,6,21,10,
            23,19,12,4,26,8,
            16,7,27,20,13,2,
            41,52,31,37,47,55,
            30,40,51,45,33,48,
            44,49,39,56,34,53,
            46,42,50,36,29,32
        };
        static byte[] E = {
            32,1,2,3,4,5,
            4,5,6,7,8,9,
            8,9,10,11,12,13,
            12,13,14,15,16,17,
            16,17,18,19,20,21,
            20,21,22,23,24,25,
            24,25,26,27,28,29,
            28,29,30,31,32,1
        };
        static byte[] P =
        {
            16,  7,  20,  21,
           29 , 12 , 28 , 17 ,
            1 ,15 , 23 , 26 ,
            5 ,18 , 31 , 10 ,
            2 , 8 , 24 , 14 ,
           32 ,27 ,  3 ,  9 ,
           19 ,13 , 30 ,  6 ,
           22 ,11 ,  4 , 25
        };
        static public uint ShiftLeft(uint z_value, int z_shift, int size)
        {
            return ((z_value << z_shift) | (z_value >> (size - z_shift))) & 0x0FFFFFFF;
        }
        static public ulong ShiftLeft(ulong z_value, int z_shift, int size)
        {
            return ((z_value << z_shift) | (z_value >> (size - z_shift))) & 0x0FFFFFFF;
        }
        static public uint GetBit(uint b, int n, int at)
        {
            return ((b >> n) & 1) << at;
        }
        static public ulong GetBit(ulong b, int n, int at)
        {
            return ((b >> n) & 1) << at;
        }
        static public ulong GetBitRange(ulong b, int rangeSize, int size, int at, int move = 0)
        {
            //SLOW
            ulong temp = 0;
            for (int i = 0; i < rangeSize; i++)
            {
                temp |= ((b >> at - i) & 1) << rangeSize - 1 - i;
                //temp |= GetBit(b, at-i, rangeSize-1-i);
            }
            return move != 0 ? temp << move : temp;

        }
        static public uint GetBitRange(uint b, int rangeSize, int size, int at)
        {
            //SLOW
            uint temp = 0;
            for (int i = 0; i < rangeSize; i++)
            {
                temp |= ((b >> at - i) & 1) << rangeSize - 1 - i;
                //temp |= GetBit(b, at-i, rangeSize-1-i);
            }
            return temp;
        }
        static public ulong Permutate(ulong target, int targetLength, byte[] table, bool debug = false)
        {
            ulong temp = 0;
            int tableLength = table.Length;
            for (int i = tableLength - 1, j = 0; i >= 0; i--, j++)
            {
                temp |= GetBit(target, targetLength - table[i], j);
                if (debug)
                    Console.WriteLine(Convert.ToString((long)temp, 2));
            }
            return temp;
        }
        //static public ulong Permutate(uint target, int targetLength, byte[] table, bool debug = false)
        //{
        //    ulong temp = 0;
        //    int tableLength = table.Length;
        //    for (int i = tableLength - 1, j = 0; i >= 0; i--, j++)
        //    {
        //        temp |= GetBit(target, targetLength - table[i], j);
        //        if (debug)
        //            Console.WriteLine(Convert.ToString((long)temp, 2).PadLeft(48, '0'));
        //    }
        //    return temp;
        //}
        static void Main(string[] args)
        {

            //string tempb = Convert.ToString((long)temp, 2).PadLeft(48, '0');
            //string k0b = Convert.ToString((long)k[0], 2).PadLeft(56, '0');
            //Console.WriteLine($"{tempb} {k0b}");

            /* 
             * Step 2: Encode each 64-bit block of data.
             * There is an initial permutation IP of the 64 bits of the message data M.
             * This rearranges the bits according to the following table,
             * where the entries in the table show the new arrangement of the bits from their initial order.
             * The 58th bit of M becomes the first bit of IP. The 50th bit of M becomes the second bit of IP.
             * The 7th bit of M is the last bit of IP.
             * 
             */
            //133457799BBCDFF1/5B5A57676A56676E/0123456789ABCDEF
            ulong K = 0x675A69675E5A6B5A;
            //0x675A69675E5A6B5A/0123456789ABCDEF
            ulong M = 0x5B5A57676A56676E;
            //ulong[] roundKeys = KeyGen(K);
            //ulong C=Encrypt(roundKeys,M);
            //Array.Reverse(roundKeys);
            //ulong plain= Encrypt(roundKeys, C);
            Console.WriteLine("Key:" + ToHex(K, 16, 4));
            Console.WriteLine("Message:" + ToHex(M, 16, 4));
            ulong[] roundKeys = MyDESAlgorithm.KeyGen(K);
            ulong C = MyDESAlgorithm.Encrypt(roundKeys, M);
            Console.WriteLine("Cifier:" + ToHex(C, 16, 4));
            Array.Reverse(roundKeys);
            ulong plain = MyDESAlgorithm.Encrypt(roundKeys, C);
            Console.WriteLine("Plain:" + ToHex(plain,16,4));


        }

        public static ulong[] KeyGen(ulong K)
        {

           
            //Console.WriteLine("K: " + Convert.ToString((long)K, 2).PadLeft(64, '0'));
            ulong temp = 0;
            for (int i = 55, j = 0; i >= 0; i--, j++)
            {
                temp |= GetBit(K, 64 - PC1[i], j);
            }
            K = temp;
            //Console.WriteLine("K+: " + Convert.ToString((long)K, 2).PadLeft(56, '0'));

            uint l = (uint)(K >> 28);
            uint r = (uint)(K & 0x0FFFFFFF);
            string lb = Convert.ToString(l, 2).PadLeft(28, '0');
            string rb = Convert.ToString(r, 2).PadLeft(28, '0');
            //Console.WriteLine("halves:  " + lb + " " + rb);
            uint[] lk = new uint[16];
            uint[] rk = new uint[16];
            int[] keyShiftTable = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };
            ulong[] k = new ulong[16];

            for (int i = 0; i < 16; i++)
            {
                //result1 = (result1 << s | result1 >> (28 - s)) & 0x0FFFFFFF;
                //binary1 = Convert.ToString(result1, 2).PadLeft(28, '0');

                lk[i] = ShiftLeft(i == 0 ? l : lk[i - 1], keyShiftTable[i], 28);
                rk[i] = ShiftLeft(i == 0 ? r : rk[i - 1], keyShiftTable[i], 28);
                k[i] = lk[i];
                k[i] <<= 28;
                k[i] |= rk[i];
                lb = Convert.ToString(lk[i], 2).PadLeft(28, '0');
                rb = Convert.ToString(rk[i], 2).PadLeft(28, '0');
                string kb = Convert.ToString((long)k[i], 2).PadLeft(56, '0');
                //Console.WriteLine($"row[{i}]:{(i / 10 < 1 ? "  " : " ")}{lb} {rb} {kb}");
                //binary2 = Convert.ToString(result2, 2).PadLeft(28, '0');
            }

            //var tempb = Convert.ToString((long)temp, 2).PadLeft(48, '0');
            //Console.WriteLine(tempb);
            for (int ikey = 0; ikey < 16; ikey++)
            {
                temp = 0;
                for (int i = 47, j = 0; i >= 0; i--, j++)
                {
                    temp |= GetBit(k[ikey], 56 - PC2[i], j);
                }
                k[ikey] = temp;
                //Console.WriteLine($"k[{ikey}]:{(ikey / 10 < 1 ? "    " : "   ")}" + Convert.ToString((long)temp, 2).PadLeft(48, '0'));
            }
            return k;
        }
        public static ulong Encrypt(ulong[] key,ulong M)
        {
            ulong MAfterIP = Permutate(M, 64, IP);
            Console.WriteLine("M:  " + Convert.ToString((long)M, 2).PadLeft(64, '0'));
            //Console.WriteLine("IP: " + Convert.ToString((long)MAfterIP, 2).PadLeft(64, '0'));
            Console.WriteLine("IP: 0x" + Convert.ToString((long)MAfterIP, 16));
            uint LM = (uint)(MAfterIP >> 32);
            uint RM = (uint)(MAfterIP & 0xFFFFFFFF);
            //string lmb = Convert.ToString(LM, 2).PadLeft(32, '0');
            string lmb = Convert.ToString(LM, 16).PadLeft(8, '0');
            //string rmb = Convert.ToString(RM, 2).PadLeft(32, '0');
            string rmb = Convert.ToString(RM, 16).PadLeft(8, '0');
            Console.WriteLine(lmb + " 0x" + rmb);
            uint prevRM = RM;
            uint prevLM = LM;
            //Console.WriteLine("LM[0]: " + Convert.ToString((long)prevLM, 2).PadLeft(32, '0'));
            //Console.WriteLine("RM[0]: " + Convert.ToString((long)prevRM, 2).PadLeft(32, '0'));

            for (int i = 0; i < 16; i++)
            {
                Console.WriteLine($"LM[{i}]: " + /*ToHex(LM, 16, 2)+" "+*/ ToBinary(LM, 32, 4));
                Console.WriteLine($"RM[{i}]: " + /*ToHex(RM, 16, 2)+" "+*/ ToBinary(RM, 32, 4));
                LM = prevRM;
                //Ln = Rn-1
                //Rn = Ln-1 + f(Rn-1,Kn)
                uint f =  F(prevRM, key[i]);
                RM = prevLM ^ f;
                prevLM = LM;
                prevRM = RM;
                Console.WriteLine($"k[{i}]:  " + /*ToHex(key[i], 16, 2)+" "+*/ ToBinary(key[i], 48, 6));
                Console.WriteLine($"f: " + /*ToHex(LM, 16, 2)+" "+*/ ToBinary(LM, 32, 4));
               
                //Console.WriteLine($"k[{i}]: " + String.Join(" ", Convert.ToString((long)key[i], 2).PadLeft(48, '0').SplitInParts(6)));
                //Console.WriteLine($"LM[{i}]: " + String.Join(" ", Convert.ToString(LM, 2).PadLeft(32, '0').SplitInParts(4)));
                //Console.WriteLine($"RM[{i}]: " + String.Join(" ", Convert.ToString(RM, 2).PadLeft(32, '0').SplitInParts(4)));
            }
            
            Console.WriteLine($"LM: " + /*ToHex(LM, 16, 2)+" "+*/ ToBinary(LM, 32, 4));
            Console.WriteLine($"RM: " + /*ToHex(RM, 16, 2)+" "+*/ ToBinary(RM, 32, 4));
            ulong RL = ((ulong)RM<<32)|LM;
            Console.WriteLine($"RL: " + /*ToHex(RM, 16, 2)+" "+*/ ToBinary(RL, 64, 4));
            RL = Permutate(RL, 64, IPReverse);
            Console.WriteLine($"IPINV(RL): " + /*ToHex(RM, 16, 2)+" "+*/ ToBinary(RL, 64, 4));
            Console.WriteLine($"C: " + /*ToHex(RM, 16, 2)+" "+*/ ToHex(RL, 16, 4));
            return RL;

        }
       /* public static ulong Decrypt(ulong[] key,ulong M)
        {
           
            ulong MAfterIP = Permutate(M, 64, IP);
            Console.WriteLine("M:  " + ToHex(M,16,4));
            //Console.WriteLine("IP: " + Convert.ToString((long)MAfterIP, 2).PadLeft(64, '0'));
            Console.WriteLine("IP: 0x" + Convert.ToString((long)MAfterIP, 16));
            uint LM = (uint)(MAfterIP >> 32);
            uint RM = (uint)(MAfterIP & 0xFFFFFFFF);
            //string lmb = Convert.ToString(LM, 2).PadLeft(32, '0');
            string lmb = Convert.ToString(LM, 16).PadLeft(8, '0');
            //string rmb = Convert.ToString(RM, 2).PadLeft(32, '0');
            string rmb = Convert.ToString(RM, 16).PadLeft(8, '0');
            Console.WriteLine(lmb + " 0x" + rmb);
            uint prevRM = RM;
            uint prevLM = LM;
            //Console.WriteLine("LM[0]: " + Convert.ToString((long)prevLM, 2).PadLeft(32, '0'));
            //Console.WriteLine("RM[0]: " + Convert.ToString((long)prevRM, 2).PadLeft(32, '0'));

            for (int i = 15; i > 0; i--)
            {
                Console.WriteLine($"LM[{i}]: " + *//*ToHex(LM, 16, 2)+" "+*//* ToBinary(LM, 32, 4));
                Console.WriteLine($"RM[{i}]: " + *//*ToHex(RM, 16, 2)+" "+*//* ToBinary(RM, 32, 4));
                LM = prevRM;
                //Ln = Rn-1
                //Rn = Ln-1 + f(Rn-1,Kn)
                uint f = F(prevRM, key[i]);
                RM = prevLM ^ f;
                prevLM = LM;
                prevRM = RM;
                Console.WriteLine($"k[{i}]:  " + *//*ToHex(key[i], 16, 2)+" "+*//* ToBinary(key[i], 48, 6));
                Console.WriteLine($"f: " + *//*ToHex(LM, 16, 2)+" "+*//* ToBinary(LM, 32, 4));

                //Console.WriteLine($"k[{i}]: " + String.Join(" ", Convert.ToString((long)key[i], 2).PadLeft(48, '0').SplitInParts(6)));
                //Console.WriteLine($"LM[{i}]: " + String.Join(" ", Convert.ToString(LM, 2).PadLeft(32, '0').SplitInParts(4)));
                //Console.WriteLine($"RM[{i}]: " + String.Join(" ", Convert.ToString(RM, 2).PadLeft(32, '0').SplitInParts(4)));
            }

            Console.WriteLine($"LM: " + *//*ToHex(LM, 16, 2)+" "+*//* ToBinary(LM, 32, 4));
            Console.WriteLine($"RM: " + *//*ToHex(RM, 16, 2)+" "+*//* ToBinary(RM, 32, 4));
            ulong RL = ((ulong)RM << 32) | LM;
            Console.WriteLine($"RL: " + *//*ToHex(RM, 16, 2)+" "+*//* ToBinary(RL, 64, 4));
            RL = Permutate(RL, 64, IPReverse);
            Console.WriteLine($"IPINV(RL): " + *//*ToHex(RM, 16, 2)+" "+*//* ToBinary(RL, 64, 4));
            Console.WriteLine($"Plain: " + *//*ToHex(RM, 16, 2)+" "+*//* ToHex(RL, 16, 4));
            return RL;
        }*/
        private static ulong S(ulong b, int sboxIdx, bool debug = false)
        {
            ulong i = GetBit(b, 5, 1) | GetBit(b, 0, 0);
            ulong j = GetBitRange(b, 4, 6, 4);
            var temp = SBOX[sboxIdx][i][j];
            if (debug)
                Console.WriteLine($"SBOX[i][{i},{j}]=> {temp}");

            return temp;
        }
        private static uint F(uint R, ulong k)
        {
            //Console.WriteLine("RBeforeE: " + Convert.ToString((long)R, 2).PadLeft(32, '0'));
            ulong RAfterE = Permutate(R, 32, E);
            Console.WriteLine("E(R): " + ToBinary(RAfterE,48,6));
            ulong temp = k ^ RAfterE;
            Console.WriteLine("k+E(R): " + ToBinary(temp, 48, 6));
            ulong b, s = 0;
            for (int i = 0; i < 8; i++)
            {
                b = GetBitRange(temp, 6, 48 - 6 * i, 47 - 6 * i);
                s |= S(b, i) << (7 - i) * 4;

            }
            //f = P(S1(B1)S2(B2)...S8(B8))
            uint f = (uint)Permutate(s, 32, P);
            Console.WriteLine("s:" + ToBinary(s, 32, 4));
            Console.WriteLine("f:" + ToBinary(f, 32, 4));
            //Console.WriteLine("s: "+String.Join(" ", Convert.ToString((long)s, 2).PadLeft(32, '0').SplitInParts(4)));
            //Console.WriteLine("f(R[i] , K[i] ): "+String.Join(" ", Convert.ToString((long)f, 2).PadLeft(32, '0').SplitInParts(4)));
            return f;
            //var t = GetBitRange(0b01110111000111, 7, 8, 12);
            //Console.WriteLine("temp: " + Convert.ToString((long)t, 2).PadLeft(8, '0'));
        }
        public static string ToBinary(ulong b, int pad, int split)
        {
            return string.Join(" ", Convert.ToString((long)b, 2).PadLeft(pad, '0').SplitInParts(split));
        }
        public static string ToHex(ulong b, int pad, int split)
        {
            return string.Join(" ", Convert.ToString((long)b, 16).PadLeft(pad, '0').SplitInParts(split));
        }
    }
    static class StringExtensions
    {

        public static IEnumerable<String> SplitInParts(this String s, Int32 partLength)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", nameof(partLength));

            for (var i = 0; i < s.Length; i += partLength)
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
        }

    }
}
