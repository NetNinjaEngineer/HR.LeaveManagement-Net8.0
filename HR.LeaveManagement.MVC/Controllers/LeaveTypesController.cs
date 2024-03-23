using AutoMapper;
using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HR.LeaveManagement.MVC.Controllers
{
    [Authorize(Roles = "Adminstrator")]
    public class LeaveTypesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILeaveTypeService _leaveTypeService;
        private readonly ILeaveAllocationService _leaveAllocationService;

        public LeaveTypesController(ILeaveTypeService leaveTypeService, IMapper mapper, ILeaveAllocationService leaveAllocationService)
        {
            _leaveTypeService = leaveTypeService;
            _mapper = mapper;
            _leaveAllocationService = leaveAllocationService;
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
        public async Task<IActionResult> Create([FromForm] LeaveTypeVM leaveTypeVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var createLeaveTypeVM = _mapper.Map<LeaveTypeVM, CreateLeaveTypeVM>(leaveTypeVM);
                    var response = await _leaveTypeService.CreateLeaveType(leaveTypeVM);
                    if (response.Success)
                    {
                        var responseMessage = response.Message;
                        TempData["Message"] = responseMessage;
                        return RedirectToAction(nameof(Index));
                    }
                    else
                        ModelState.AddModelError(string.Empty, response.ValidationErrors!);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(leaveTypeVM);

        }

        [HttpGet]
        public async Task<IActionResult> Details([FromRoute] int id, string viewName = "Details")
        {
            var leaveType = await _leaveTypeService.GetLeaveTypeDetails(id);
            return View(viewName, leaveType);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
            => await Details(id, nameof(Delete));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, LeaveTypeVM leaveTypeVM)
        {
            if (id != leaveTypeVM.Id)
                return BadRequest();

            var response = await _leaveTypeService.DeleteLeaveType(leaveTypeVM.Id);
            if (response.Success)
            {
                return RedirectToAction(nameof(Index));
            }
            else
                ModelState.AddModelError(string.Empty, response.ValidationErrors!);
            return View(leaveTypeVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
            => await Details(id, nameof(Edit));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, LeaveTypeVM leaveTypeVM)
        {
            if (id != leaveTypeVM.Id)
                return BadRequest();

            var response = await _leaveTypeService.UpdateLeaveType(id, leaveTypeVM);

            if (response.Success)
                return RedirectToAction(nameof(Index));
            else
                ModelState.AddModelError("", response.ValidationErrors!);

            return View(leaveTypeVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Allocate(int id)
        {
            try
            {
                var response = await _leaveAllocationService.CreateLeaveAllocation(id);
                if (response.Success)
                    return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View();
        }

    }
}
