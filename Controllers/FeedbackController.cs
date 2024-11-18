using ecommerce.Dtos.FeedbackDtos;
using ecommerce.Interfaces;
using ecommerce.Mappers;
using ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackRepository _feedbackRepo;
        private readonly IProductRepository _productRepo;
        private readonly IUserService _userService;
        public FeedbackController(IFeedbackRepository feedbackRepo, IProductRepository productRepo, IUserService userService)
        {
            _feedbackRepo = feedbackRepo;
            _productRepo = productRepo;
            _userService = userService;
        }

        [HttpGet("GetFeedbacksByProductId/{id}")]
        public async Task<IActionResult> GetFeedbacksByProductId([FromRoute] string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await _productRepo.ProductExist(id)) return NotFound("Product not found");
            var feedbacks = await _feedbackRepo.GetFeedbacksByProductIdAsync(id);

            return Ok(feedbacks.Select(f => f.ToFeedbackDto()));
        }

        [HttpGet("GetFeedbacksByCustomerId/{id}")]
        public async Task<IActionResult> GetFeedbacksByCustomerId([FromRoute] string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var feedbacks = await _feedbackRepo.GetFeedbackByCustomerIdAsync(id);

            return Ok(feedbacks.Select(f => f.ToFeedbackDto()));
        }

        [HttpPost("CreateFeedback/{productId}")]
        [Authorize]
        public async Task<IActionResult> CreateFeedback([FromRoute] string productId, [FromForm] CreateFeedbackRequestDto feedbackDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await _productRepo.ProductExist(productId)) return NotFound("Product does not exist");

            var appUser = await _userService.GetUserAsync();
            if (appUser == null) return Unauthorized("User not found");

            var feedbackModel = feedbackDto.ToFeedbackFromCreateDto(productId, appUser.Id);

            await _feedbackRepo.CreateFeedbackAsync(feedbackModel);
            return Ok(feedbackModel.ToFeedbackDto());
        }

        [HttpPut("UpdateFeedback/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateFeedback([FromRoute] string id, [FromForm] UpdateFeedbackRequestDto feedbackDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var appUser = await _userService.GetUserAsync();
            if (appUser == null) return Unauthorized("User not found");

            var (feedbackModel, isAuth) = await _feedbackRepo.UpdateFeedbackAsync(id, feedbackDto, appUser.Id);

            if (isAuth == false) return Unauthorized("Do not have authorization to edit this feedback");
            if (feedbackModel == null) return NotFound("Feedback not found");

            return Ok(feedbackModel.ToFeedbackDto());
        }

        [HttpDelete("DeleteFeedbackById/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteFeedbackAsync(string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (User.IsInRole("Admin"))
            {
                var feedback = await _feedbackRepo.AdminDeleteFeedbackAsync(id);
                if (feedback == null) return NotFound("Feedback not found");

                return Ok("Delete successfully");
            }

            var appUser = await _userService.GetUserAsync();
            if (appUser == null) return Unauthorized("User not found");

            var (feedbackModel, isAuth) = await _feedbackRepo.CustomerDeleteFeedbackAsync(id, appUser.Id);

            if (isAuth == false) return Unauthorized("Do not have authorization to delete this feedback");
            if (feedbackModel == null) return NotFound("Feedback not found");

            return Ok("Delete successfully");
        }
    }
}