namespace SuperCircle.Interfaces
{
    public interface ProductionOrderIdInterface
    {
        string GenerateProdId(string type,int stage,string order,string lastId);
    }

    public class ProductionOrderId : ProductionOrderIdInterface
    {

        private readonly int _totalDigits;

        public ProductionOrderId(int totalDigits = 10)
        {
            _totalDigits = totalDigits;
        }
        public string GenerateProdId(string type, int stage,string order, string lastId)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("Type cannot be null or empty", nameof(type));

            // Ensure type is exactly 2 characters (01, 02, etc.)
            type = type.PadLeft(2, '0');

            // Prefix = type + stage (e.g., 01 + 1 → 011)
            string prefix = $"{type}{stage}";

            const int sequenceLength = 10;
            long nextSequence = 1;

            if (!string.IsNullOrWhiteSpace(lastId) && lastId.Length >= prefix.Length)
            {
                // Extract numeric part AFTER prefix
                string numberPart = lastId.Substring(prefix.Length);

                if (long.TryParse(numberPart, out long lastNumber))
                {
                    nextSequence = lastNumber + 1;
                }
            }

            return $"{prefix}{nextSequence.ToString().PadLeft(sequenceLength, '0')}";
        }

    }
    public static class IDFactoryProd
    {
        public static ProductionOrderIdInterface Create(string type)
        {
            return type?.ToLower() switch
            {

                "production" => new ProductionOrderId(),

                _ => throw new ArgumentException("Unknown type")
            };
        }
    }
}
