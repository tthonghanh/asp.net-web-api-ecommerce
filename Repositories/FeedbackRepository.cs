using ecommerce.Data;
using ecommerce.Dtos.FeedbackDtos;
using ecommerce.Interfaces;
using ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly ApplicationDbContext _context;
        public FeedbackRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Feedback?> AdminDeleteFeedbackAsync(string id)
        {
            var feedbackModel = await _context.Feedbacks.FirstOrDefaultAsync(f => f.Id == id);

            if (feedbackModel == null) return null;

            _context.Remove(feedbackModel);
            await _context.SaveChangesAsync();

            return feedbackModel;
        }

        public async Task<Feedback> CreateFeedbackAsync(Feedback feedbackModel)
        {
            await _context.Feedbacks.AddAsync(feedbackModel);
            await _context.SaveChangesAsync();
            return feedbackModel;
        }

        public async Task<(Feedback?, bool?)> CustomerDeleteFeedbackAsync(string id, string appUserId)
        {
            var feedbackModel = await _context.Feedbacks.FirstOrDefaultAsync(f => f.Id == id);

            if (feedbackModel == null) return (null, null);
            if (feedbackModel.AppUserId != appUserId) return (null, false);

            _context.Remove(feedbackModel);
            await _context.SaveChangesAsync();

            return (feedbackModel, true);
        }

        public async Task<List<Feedback>> GetFeedbackByCustomerIdAsync(string appUserId)
        {
            return await _context.Feedbacks
                                    .Where(f => f.AppUserId == appUserId)
                                    .Include(f => f.AppUser)
                                    .Include(f => f.Product)
                                    .ToListAsync();
        }

        public async Task<List<Feedback>> GetFeedbacksByProductIdAsync(string productId)
        {
            return await _context.Feedbacks
                                .Include(f => f.AppUser)
                                .Where(f => f.ProductId == productId)
                                .ToListAsync();
        }

        public async Task<(Feedback?, bool?)> UpdateFeedbackAsync(string id, UpdateFeedbackRequestDto feedbackDto, string appUserId)
        {
            var existingFeedback = await _context.Feedbacks.FirstOrDefaultAsync(f => f.Id == id);

            if (existingFeedback == null) return (null, null);
            if (existingFeedback.AppUserId != appUserId) return (null, false);

            existingFeedback.Content = feedbackDto.Content;
            existingFeedback.Stars = feedbackDto.Stars;

            await _context.SaveChangesAsync();
            return (existingFeedback, true);
        }
    }
}