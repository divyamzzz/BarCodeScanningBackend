using System;
using System.Diagnostics;
using System.Numerics;

namespace SuperCircle.Interfaces
{
    public interface GenerateIdInterface
    {
        string generateId(string lastId);
    }

    public class RouteCardId : GenerateIdInterface
    {

        private readonly int _totalDigits;

        public RouteCardId(int totalDigits = 7)
        {
            _totalDigits = totalDigits;
        }
        public string generateId(string lastId)
        {
            string prefix = "RC";
            if (string.IsNullOrEmpty(prefix))
                throw new ArgumentException("Prefix cannot be null or empty", nameof(prefix));

            int nextNumber = 1;

            if (!string.IsNullOrEmpty(lastId))
            {
                // Extract numeric part after prefix
                string numberPart = new string(lastId.SkipWhile(c => !char.IsDigit(c)).ToArray());

                if (int.TryParse(numberPart, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            return $"{prefix}{nextNumber.ToString().PadLeft(_totalDigits, '0')}";
        }
    }
    public static class IDFactory
    {
        public static GenerateIdInterface Create(string type)
        {
            return type?.ToLower() switch
            {

                "route" => new RouteCardId(),
                
                _ => throw new ArgumentException("Unknown type")
            };
        }
    }
}
