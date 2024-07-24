using ecommerce.Models;

namespace ecommerce.Extensions
{
    public static class ProductExtension
    {
        public static decimal GetAverageFeedbackStars(this Product productModel) {
            if (productModel.Feedbacks.Count == 0) return 0;

            var totalStar = 0;
            foreach (var feedback in productModel.Feedbacks)
            {
                totalStar += feedback.Stars;
            }

            return totalStar / productModel.Feedbacks.Count;
        }
    }
}