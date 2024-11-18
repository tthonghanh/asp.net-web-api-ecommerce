using ecommerce.Dtos.FeedbackDtos;
using ecommerce.Models;

namespace ecommerce.Mappers
{
    public static class FeedbackMapper
    {
        public static FeedbackDto ToFeedbackDto(this Feedback feedbackModel)
        {
            return new FeedbackDto
            {
                Id = feedbackModel.Id,
                Content = feedbackModel.Content,
                Stars = feedbackModel.Stars,
                CreateAt = feedbackModel.CreateAt,
                CreateBy = feedbackModel.AppUser?.UserName,
            };
        }

        public static Feedback ToFeedbackFromCreateDto(this CreateFeedbackRequestDto feedbackDto, string productId, string userId)
        {
            return new Feedback
            {
                Content = feedbackDto.Content,
                Stars = feedbackDto.Stars,
                ProductId = productId,
                AppUserId = userId
            };
        }
    }
}