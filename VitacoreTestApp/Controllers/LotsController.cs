using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VitacoreTestApp.Services;
using VitacoreTestApp.ViewModels;

namespace VitacoreTestApp.Controllers
{
    public class LotsController : Controller
    {
        private readonly AuctionService _auctionService;

        public LotsController(AuctionService auctionService)
        {
            _auctionService = auctionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var lots = await _auctionService.GetActiveLotsAsync();
            return View(lots);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var lot = await _auctionService.GetLotWithBidsAsync(id);
            if (lot == null)
                return NotFound();

            User? currentUser = null;
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
                currentUser = await _auctionService.GetUserAsync(int.Parse(userIdClaim.Value));

            var viewModel = new LotDetailsViewModel
            {
                Lot = lot,
                CurrentUser = currentUser,
                Bids = lot.Bids.OrderByDescending(b => b.Timestamp).ToList(),
                UserHighestBid = lot.Bids
                    .Where(b => b.UserId == currentUser?.Id)
                    .Select(b => b.Amount)
                    .DefaultIfEmpty(0)
                    .Max()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceBid(int lotId, decimal amount)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                TempData["Error"] = "You must be logged in to place a bid.";
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdClaim.Value);
            var (success, message) = await _auctionService.PlaceBidAsync(lotId, userId, amount);

            if (success)
            {
                TempData["Success"] = "Your bid was placed successfully!";

                if (!string.IsNullOrEmpty(message))
                    TempData["Info"] = "📧 The previous top bidder has been notified by email.";
            }
            else
            {
                TempData["Error"] = message;
            }

            return RedirectToAction("Details", new { id = lotId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buyout(int lotId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                TempData["Error"] = "You must be logged in to buy out a lot.";
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdClaim.Value);
            var (success, message) = await _auctionService.BuyoutAsync(lotId, userId);

            if (success)
                TempData["Success"] = $"🎉 {message} 📧 A receipt has been sent to your email.";
            else
                TempData["Error"] = message;

            return RedirectToAction("Details", new { id = lotId });
        }
    }
}