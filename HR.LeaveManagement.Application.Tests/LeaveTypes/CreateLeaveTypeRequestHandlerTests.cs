using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Application.Profiles;
using HR.LeaveManagement.Application.Responses;
using HR.LeaveManagement.Domain;
using Moq;
using Shouldly;

namespace HR.LeaveManagement.Application.Tests.LeaveTypes
{
    public class CreateLeaveTypeRequestHandlerTests
    {
        private readonly Mock<ILeaveTypeRepository> _leaveTypeMockRepository;
        private readonly IMapper _mapper;

        public CreateLeaveTypeRequestHandlerTests()
        {
            var leaveTypes = new List<LeaveType>()
            {
                new() {
                    Name = "Vacation Test",
                    DefaultDays = 10
                },
                new() {
                    Name = "Sick Test",
                    DefaultDays = 20
                }
            };

            _leaveTypeMockRepository = new Mock<ILeaveTypeRepository>();

            _leaveTypeMockRepository.Setup(repo => repo.Add(It.IsAny<LeaveType>()).Result)
                .Returns((LeaveType leaveType) =>
                {
                    leaveTypes.Add(leaveType);
                    return leaveType;
                });

            var _mapperConfiguration = new MapperConfiguration(config =>
                config.AddProfile(typeof(MappingProfile)));

            _mapper = _mapperConfiguration.CreateMapper();
        }

        [Fact]
        public async Task Handle_ValidCreate_ShouldReturnsCreateCommandResponse()
        {
            var command = new CreateLeaveTypeCommand
            {
                CreateLeaveTypeDto = new DTOs.LeaveType.CreateLeaveTypeDto
                {
                    Name = "Test Create",
                    DefaultDays = 10
                }
            };

            var handler = new CreateLeaveTypeRequestHandler(
                _leaveTypeMockRepository.Object,
                _mapper);

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBeOfType<CreateCommandResponse>();
        }

    }
}
