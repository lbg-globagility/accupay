using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SettingsController : ControllerBase
    {
        private readonly SettingService _settingService;
        private readonly ICurrentUser _currentUser;

        public SettingsController(SettingService settingService, ICurrentUser currentUser)
        {
            _settingService = settingService;
            _currentUser = currentUser;
        }

        [HttpGet("web-settings")]
        [Permission(PermissionTypes.SettingsRead)]
        public async Task<ActionResult<List<SettingDto>>> GetWebSettings()
        {
            return await _settingService.GetWebSettingPolicy();
        }

        [HttpPut("web-settings/{id}")]
        [Permission(PermissionTypes.SettingsUpdate)]
        public async Task<ActionResult<SettingDto>> UpdateWebSetting(int id, [FromBody] SettingDto setting)
        {
            return await _settingService.UpdateWebSetting(id, setting, _currentUser.UserId);
        }
    }
}
