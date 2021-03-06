﻿using System;
using System.Linq;

namespace PKHeX.Core
{
    public class SecretBase3
    {
        private readonly byte[] Data;
        private readonly int Offset;
        private bool Japanese => Language == (int) LanguageID.Japanese;

        public SecretBase3(byte[] data, int offset)
        {
            Data = data;
            Offset = offset;
        }

        public int SecretBaseLocation { get => Data[Offset + 0]; set => Data[Offset + 0] = (byte) value; }

        public int OT_Gender
        {
            get => (Data[Offset + 1] >> 4) & 1;
            set => Data[Offset + 1] = (byte) ((Data[Offset + 1] & 0xEF) | (value & 1) << 4);
        }

        public string OT_Name
        {
            get => StringConverter.GetString3(Data, Offset + 2, 7, Japanese);
            set => StringConverter.SetString3(value, 7, Japanese, 7).CopyTo(Data, Offset + 2);
        }

        public uint OT_ID
        {
            get => BitConverter.ToUInt32(Data, Offset + 9);
            set => BitConverter.GetBytes(value).CopyTo(Data, Offset + 9);
        }

        public int OT_Class { get => Data[Offset + 9] % 5; }
        public int Language { get => Data[Offset + 0x0D]; set => Data[Offset + 0x0D] = (byte)value; }
        public int _E       { get => Data[Offset + 0x0E]; set => Data[Offset + 0x0E] = (byte)value; }
        public int _F       { get => Data[Offset + 0x0F]; set => Data[Offset + 0x0F] = (byte)value; }
        public int _10      { get => Data[Offset + 0x10]; set => Data[Offset + 0x10] = (byte)value; }
        public int _11      { get => Data[Offset + 0x11]; set => Data[Offset + 0x11] = (byte)value; }

        public byte[] Decorations
        {
            get => Data.Skip(Offset + 0x12).Take(0x10).ToArray();
            set => value.CopyTo(Data, Offset + 0x12);
        }

        public byte[] DecorationCoordinates
        {
            get => Data.Skip(Offset + 0x22).Take(0x10).ToArray();
            set => value.CopyTo(Data, Offset + 0x22);
        }

        public SecretBase3Team Team
        {
            get => new SecretBase3Team(Data.Skip(Offset + 50).Take(72).ToArray());
            set => value.Write().CopyTo(Data, Offset + 50);
        }

        public int TID
        {
            get => (ushort)OT_ID;
            set => OT_ID = (ushort)(SID | (ushort)value);
        }

        public int SID
        {
            get => (ushort)OT_ID >> 8;
            set => OT_ID = (ushort)(((ushort)value << 16) | TID);
        }
    }
}
