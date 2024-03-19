using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace HR.LeaveManagement.MVC.Controllers
{
    public class LeaveTypesController : Controller
    {
        private readonly ILeaveTypeService _leaveTypeService;

        public LeaveTypesController(ILeaveTypeService leaveTypeService)
        {
            _leaveTypeService = leaveTypeService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var leaveTypes = await _leaveTypeService.GetLeaveTypes();
            return View(leaveTypes);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateLeaveTypeVM leaveTypeVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _leaveTypeService.CreateLeaveType(leaveTypeVM);
                    if (response.Success)
                    {
                        var responseMessage = response.Message;
                        TempData["Message"] = "LeaveType Created Successfully";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                        ModelState.AddModelError(string.Empty, response.ValidationErrors ?? "Invalid Data");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(leaveTypeVM);

        }
    }
}
