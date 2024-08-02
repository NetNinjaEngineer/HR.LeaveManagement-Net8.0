using AutoMapper;
using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Services.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HR.LeaveManagement.MVC.Controllers;


public class LeaveRequestsController : Controller
{
    private readonly ILeaveTypeService _leaveTypeService;
    private readonly ILeaveRequestService _leaveRequestService;
    private readonly IMapper _mapper;

    public LeaveRequestsController(ILeaveTypeService leaveTypeService, ILeaveRequestService leaveRequestService, IMapper mapper)
    {
        _leaveTypeService = leaveTypeService;
        _leaveRequestService = leaveRequestService;
        _mapper = mapper;
    }

    // GET: LeaveRequestsController
    [Authorize(Roles = "Adminstrator")]
    public async Task<IActionResult> Index()
    {
        var model = await _leaveRequestService.GetAdminLeaveRequestList();
        return View(model);
    }

    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> MyLeave()
    {
        var model = await _leaveRequestService.GetEmployeeLeaveRequests();
        return View(model);
    }

    // GET: LeaveRequestsController/Details/5
    [Authorize(Roles = "Adminstrator")]
    public async Task<IActionResult> Details(int id)
    {
        var leaveRequest = await _leaveRequestService.GetLeaveRequest(id);
        return View(leaveRequest);
    }

    // GET: LeaveRequestsController/Create
    public async Task<IActionResult> Create()
    {
        var leaveTypes = await _leaveTypeService.GetLeaveTypes();
        var leaveTypeItems = new SelectList(leaveTypes, "Id", "Name");
        var model = new CreateLeaveRequestVM
        {
            LeaveTypes = leaveTypeItems
        };

        return View(model);
    }

    // POST: LeaveRequestsController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateLeaveRequestVM leaveRequestVM)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var response = await _leaveRequestService.CreateLeaveRequest(leaveRequestVM);
                if (response.Success)
                    return RedirectToAction(nameof(Create));
                else
                    ModelState.AddModelError("", response.ValidationErrors!);
            }

            var leaveTypes = await _leaveTypeService.GetLeaveTypes();
            var leaveTypeItems = new SelectList(leaveTypes, "Id", "Name");
            leaveRequestVM.LeaveTypes = leaveTypeItems;

            return View(leaveRequestVM);

        }
        catch (ApiException ex)
        {
            ModelState.AddModelError("", ex.Response);
            return View();
        }
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Adminstrator")]
    public async Task<IActionResult> ApproveRequest(int id, bool approved)
    {
        try
        {
            await _leaveRequestService.ApproveLeaveRequest(id, approved);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
