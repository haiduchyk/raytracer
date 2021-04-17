namespace ConsoleApplication
{
    using System;

    public interface IProgressBar
    {
        void Draw(int progress, int total, string info);
    }
    
    public class ProgressBar : IProgressBar
    {
        private const int Size = 20;
        private int position;

        public void Draw(int progress, int total, string info)
        {
            Console.CursorLeft = 0;

            Console.Write($"{info}:");
            position = info.Length + 1;

            var onechunk = (float) Size / total;

            var amount = onechunk * progress;

            for (var i = 0; i < amount; i++)
            {
                Print('■');
            }

            if (progress >= total) return;

            for (var i = 1; i < Size - Math.Ceiling(amount) + 1; i++)
            {
                Print('-');
            }

            Console.CursorLeft = position++;
        }

        private void Print(char sym)
        {
            position++;
            Console.Write(sym);
        }
    }
}