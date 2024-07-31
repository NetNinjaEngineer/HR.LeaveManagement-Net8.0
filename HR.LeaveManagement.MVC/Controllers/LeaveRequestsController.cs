using AutoMapper;
using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Services.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HR.LeaveManagement.MVC.Controllers;

[Authorize]
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
    public async Task<ActionResult> Index()
    {
        return View();
    }

    // GET: LeaveRequestsController/Details/5
    public ActionResult Details(int id)
    {
        return View();
    }

    // GET: LeaveRequestsController/Create
    public async Task<ActionResult> Create()
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
    public async Task<ActionResult> Create(CreateLeaveRequestVM leaveRequestVM)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var response = await _leaveRequestService.CreateLeaveRequest(leaveRequestVM);
                if (response.Success)
                    return Redirect(nameof(Index));
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

    // GET: LeaveRequestsController/Edit/5
    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: LeaveRequestsController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: LeaveRequestsController/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: LeaveRequestsController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
}
