using System;

namespace Glitch9.AIDevKit
{
    [Serializable]
    public class ModelPrice
    {
        public UsageType type;
        public double cost;
        public bool isEstimated;

        public ModelPrice() { }
        public ModelPrice(UsageType type, double cost, bool isEstimated = false)
        {
            this.type = type;
            this.cost = cost;
            this.isEstimated = isEstimated;
        }

        public static ModelPrice[] Free()
            => new[] { new ModelPrice(UsageType.Free, 0) };
        public static ModelPrice[] PerCharacter(double cost)
            => new[] { new ModelPrice(UsageType.PerCharacter, cost) };
        public static ModelPrice[] PerRequest(double cost)
            => new[] { new ModelPrice(UsageType.PerRequest, cost) };
        public static ModelPrice[] PerMinute(double cost, bool isEstimated = false)
            => new[] { new ModelPrice(UsageType.PerMinute, cost, isEstimated) };
        public static ModelPrice[] PerInputToken(double cost)
            => new[] { new ModelPrice(UsageType.InputToken, cost) };
        public static ModelPrice[] PerInputOutput(double input, double output)
            => new[] { new ModelPrice(UsageType.InputToken, input), new ModelPrice(UsageType.OutputToken, output) };
        public static ModelPrice[] PerInputCachedInputOutput(double input, double cachedInput, double output)
            => new[] { new ModelPrice(UsageType.InputToken, input), new ModelPrice(UsageType.CachedInputToken, cachedInput), new ModelPrice(UsageType.OutputToken, output) };

        public static ModelPrice[] PerImage(double cost)
            => new[] { new ModelPrice(UsageType.Image, cost) };
    }

    public static class ModelPriceArrayExtensions
    {
        public static bool IsFree(this ModelPrice[] prices)
        {
            if (prices == null || prices.Length == 0) return false;

            foreach (var price in prices)
            {
                if (price.type == UsageType.Free) return true;
            }

            return false;
        }
    }
}