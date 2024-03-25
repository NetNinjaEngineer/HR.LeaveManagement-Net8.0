using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveType;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Queries;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Queries;
using HR.LeaveManagement.Domain;
using Moq;

namespace HR.LeaveManagement.Application.Tests.LeaveTypes
{
    public class GetLeaveTypeDetailRequestHandlerTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetLeaveTypeDetailRequestHandler _handler;
        private readonly Mock<ILeaveTypeRepository> _leaveTypeRepositoryMock;

        public GetLeaveTypeDetailRequestHandlerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _leaveTypeRepositoryMock = new Mock<ILeaveTypeRepository>();
            _handler = new GetLeaveTypeDetailRequestHandler(_leaveTypeRepositoryMock.Object, _mapperMock.Object);
        }


        [Fact]
        public async Task Handle_WithValidId_ShouldReturnLeaveTypeDto()
        {
            var leaveTypeId = 1;
            var leaveType = new LeaveType { Id = leaveTypeId, Name = "Vacation" };
            var leaveTypeDto = new LeaveTypeDto { Id = leaveTypeId, Name = "Vacation" };

            _leaveTypeRepositoryMock.Setup(repo => repo.Get(leaveTypeId)).ReturnsAsync(leaveType);

            _mapperMock.Setup(mapper => mapper.Map<LeaveType, LeaveTypeDto>(leaveType))
                .Returns(leaveTypeDto);

            var request = new GetLeaveTypeDetailRequest { Id = leaveTypeId };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(leaveTypeDto.Id, result.Id);
            Assert.Equal(leaveTypeDto.Name, result.Name);
        }

        [Fact]
        public async Task Handle_WithInvalidId_ThrowsNotFoundException()
        {
            // Arrange
            var leaveTypeId = 10;
            var leaveType = new LeaveType { Id = leaveTypeId, Name = "Vacation" };
            var leaveTypeDto = new LeaveTypeDto { Id = leaveTypeId, Name = "Vacation" };

            _leaveTypeRepositoryMock.Setup(repo => repo.Get(leaveTypeId))!.ReturnsAsync((LeaveType?)null);

            var request = new GetLeaveTypeDetailRequest { Id = leaveTypeId };

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));

        }
    }
}
