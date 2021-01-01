using AccuPay.Core.Repositories;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Leaves;
using AccuPay.Web.OfficialBusinesses;
using AccuPay.Web.Overtimes;
using AccuPay.Web.Payroll;
using AccuPay.Web.TimeEntries;
using AccuPay.Web.TimeEntries.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SelfServiceFilingController : ControllerBase
    {
        private readonly LeaveService _leaveService;
        private readonly LeaveRepository _leaveRepository;
        private readonly OvertimeService _overtimeService;
        private readonly OfficialBusinessService _officialBusinessService;
        private readonly TimeEntryService _timeEntryService;
        private readonly ICurrentUser _currentUser;
        private readonly PayperiodService _payperiodService;

        public SelfServiceFilingController(LeaveService leaveService,
                                           LeaveRepository leaveRepository,
                                           OvertimeService overtimeService,
                                           OfficialBusinessService officialBusinessService,
                                           TimeEntryService timeEntryService,
                                           ICurrentUser currentUser,
                                           PayperiodService payperiodService)
        {
            _leaveService = leaveService;
            _leaveRepository = leaveRepository;
            _overtimeService = overtimeService;
            _officialBusinessService = officialBusinessService;
            _timeEntryService = timeEntryService;
            _currentUser = currentUser;
            _payperiodService = payperiodService;
        }

        #region LEAVE

        [HttpPost("leave")]
        [Permission(PermissionTypes.SelfserveLeaveCreate)]
        public async Task<ActionResult<LeaveDto>> CreateLeave([FromBody] CreateLeaveDto dto)
        {
            dto.EmployeeId = _currentUser.EmployeeId.Value;
            return await _leaveService.Create(dto);
        }

        [HttpGet("leave-types")]
        [Permission(PermissionTypes.SelfserveLeaveRead)]
        public async Task<List<string>> GetLeaveTypes()
        {
            return await _leaveService.GetLeaveTypes();
        }

        [HttpGet("leave-statuses")]
        [Permission(PermissionTypes.SelfserveLeaveRead)]
        public ActionResult<ICollection<string>> GetLeaveStatuses()
        {
            return _leaveRepository.GetStatusList();
        }

        #endregion LEAVE

        #region OVERTIME

        [HttpPost("overtime")]
        [Permission(PermissionTypes.SelfserveOvertimeCreate)]
        public async Task<ActionResult<OvertimeDto>> CreateOvertime([FromBody] CreateOvertimeDto dto)
        {
            dto.EmployeeId = _currentUser.EmployeeId.Value;
            return await _overtimeService.Create(dto);
        }

        [HttpGet("overtime-statuses")]
        [Permission(PermissionTypes.SelfserveOvertimeRead)]
        public ActionResult<ICollection<string>> GetOvertimeStatuses()
        {
            return _overtimeService.GetStatusList();
        }

        #endregion OVERTIME

        #region OFFICIAL BUSINESS

        [HttpPost("official-business")]
        [Permission(PermissionTypes.SelfserveOfficialBusinessCreate)]
        public async Task<ActionResult<OfficialBusinessDto>> CreateOfficialBusiness([FromBody] CreateOfficialBusinessDto dto)
        {
            dto.EmployeeId = _currentUser.EmployeeId.Value;
            return await _officialBusinessService.Create(dto);
        }

        [HttpGet("official-business-statuses")]
        [Permission(PermissionTypes.SelfserveOfficialBusinessRead)]
        public ActionResult<ICollection<string>> GetOfficialBusinessStatusList()
        {
            return _officialBusinessService.GetStatusList();
        }

        #endregion OFFICIAL BUSINESS

        #region TIME ENTRY

        [HttpGet("timeentry/{payPeriodId}")]
        [Permission(PermissionTypes.SelfserveTimeEntryRead)]
        public async Task<ActionResult<ICollection<TimeEntryDto>>> GetEmployeeTimeEntryByPeriod(int payPeriodId)
        {
            int employeeId = _currentUser.EmployeeId.Value;

            var dtos = await _timeEntryService.GetTimeEntries(payPeriodId, employeeId);
            return dtos.ToList();
        }

        #endregion TIME ENTRY

        #region PAY PERIOD

        [HttpGet("year/{year}")]
        public async Task<ActionResult<ICollection<PayPeriodDto>>> GetYearlyPayPeriods(int year)
        {
            return await _payperiodService.GetYearlyPayPeriods(year);
        }

        [HttpGet("latest")]
        public async Task<ActionResult<PayPeriodDto>> GetLatest()
        {
            return await _payperiodService.GetLatest();
        }

        #endregion
    }
}
