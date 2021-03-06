namespace PKHeX.Core
{
    /// <summary>
    /// PokeStock .gst
    /// </summary>
    public sealed class Bank3 : BulkStorage
    {
        public Bank3(byte[] data) : base(data, typeof(PK3), 0)
        {
            Personal = PersonalTable.RS;
            Version = GameVersion.RS;
            HeldItems = Legal.HeldItems_RS;
        }

        public override string PlayTimeString => SaveUtil.CRC16(Data, 0, Data.Length).ToString("X4");
        protected override string BAKText => PlayTimeString;
        public override string Extension => ".gst";
        public override string Filter { get; } = "PokeStock G3 Storage|*.gst*";

        public override int BoxCount => 64;
        private const int BoxNameSize = 9;

        private int BoxDataSize => SlotsPerBox * SIZE_STORED;
        public override int GetBoxOffset(int box) => Box + (BoxDataSize * box);
        public override string GetBoxName(int box) => GetString(GetBoxNameOffset(box), BoxNameSize);
        private int GetBoxNameOffset(int box) => 0x25800 + (9 * box);

        public static Bank3 GetBank3(byte[] data) => new Bank3(data);
    }
}