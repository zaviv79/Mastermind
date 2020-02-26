namespace App
{
    class DigitCounter
    {
        public DigitCounter(int val)
        {
            Value = val;
            Counted = false;
        }

        public int Value { get; set; }
        public bool Counted { get; set; }
    }
}
