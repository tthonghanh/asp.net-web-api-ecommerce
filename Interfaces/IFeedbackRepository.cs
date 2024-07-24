using ecommerce.Dtos.FeedbackDtos;
using ecommerce.Models;

namespace ecommerce.Interfaces
{
    public interface IFeedbackRepository
    {
        Task<List<Feedback>> GetFeedbacksByProductIdAsync(string productId);
        Task<List<Feedback>> GetFeedbackByCustomerIdAsync(string appUserId);
        Task<Feedback> CreateFeedbackAsync(Feedback feedbackModel);
        Task<(Feedback?, bool?)> UpdateFeedbackAsync(string id, UpdateFeedbackRequestDto feedbackDto, string appUserId);
        Task<(Feedback?, bool?)> CustomerDeleteFeedbackAsync(string id, string appUserId);
        Task<Feedback?> AdminDeleteFeedbackAsync(string id);
    }
}