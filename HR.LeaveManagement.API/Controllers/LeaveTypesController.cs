using HR.LeaveManagement.Application.DTOs.LeaveType;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Queries;
using HR.LeaveManagement.Application.Responses;
using HR.LeaveManagement.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HR.LeaveManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveTypesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaveTypesController(IMediator mediator) => _mediator = mediator;

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LeaveTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<LeaveTypeDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LeaveTypeDto>> GetLeaveTypeWithDetailsAsync(int id)
        {
            try
            {
                var leaveTypeWithDetails = await _mediator.Send(new GetLeaveTypeDetailRequest { Id = id });
                return Ok(leaveTypeWithDetails);
            }
            catch (Exception ex)
            {
                return NotFound(new Response<LeaveTypeDto>(ex.Message));
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateCommandResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<CreateCommandResponse>> CreateLeaveTypeAsync([FromBody] CreateLeaveTypeDto createLeaveTypeDto)
        {
            var createLeaveTypeDtoCommand = new CreateLeaveTypeCommand() { CreateLeaveTypeDto = createLeaveTypeDto };
            var response = await _mediator.Send(createLeaveTypeDtoCommand);
            return response;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<LeaveTypeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<LeaveTypeDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<LeaveTypeDto>>> GetLeaveTypesWithDetailsAsync()
        {
            try
            {
                var leaveTypesWithDetails = await _mediator.Send(new GetLeaveTypeListRequest());
                return Ok(leaveTypesWithDetails);
            }
            catch (Exception ex)
            {
                return NotFound(new Response<List<LeaveTypeDto>>(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DeleteCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DeleteCommandResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DeleteCommandResponse>> DeleteLeaveTypeAsync(int id)
        {
            var response = await _mediator.Send(new DeleteLeaveTypeCommand { Id = id });
            return response;
        }

        [HttpPut("id")]
        [ProducesResponseType(typeof(UpdateCommandResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<UpdateCommandResponse>> UpdateLeaveTypeAsync(int id, [FromBody] UpdateLeaveTypeDto leaveTypeDto)
        {
            var response = await _mediator.Send(
                new UpdateLeaveTypeCommand() { LeaveTypeDto = leaveTypeDto, Id = id });
            return response;
        }
    }
}
