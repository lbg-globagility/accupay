using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Interfaces;
using AccuPay.Infrastructure.Data;
using AccuPay.Web.EmailTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Settings
{
    public class SettingService
    {
        private const string WebSettingType = "WebSetting";

        private readonly IListOfValueRepository _listOfValueRepository;
        private readonly IListOfValueDataService _listOfValueDataService;

        public SettingService(
            IListOfValueRepository listOfValueRepository,
            IListOfValueDataService listOfValueDataService)
        {
            _listOfValueRepository = listOfValueRepository;
            _listOfValueDataService = listOfValueDataService;
        }
        public async Task<List<SettingDto>> GetWebSettingPolicy()
        {
            ICollection<ListOfValue> listOfValues = await _listOfValueRepository.GetListOfValuesAsync(WebSettingType);

            return listOfValues
                .Select(l => new SettingDto
                {
                    RowID = l.RowID.Value,
                    LIC = l.LIC,
                    DisplayValue = Convert.ToBoolean(l?.DisplayValue)
                })
                .ToList();
        }

        public async Task<SettingDto> UpdateWebSetting(int id, SettingDto setting, int currentlyLoggedInUserId)
        {
            ListOfValue listOfValue = await _listOfValueRepository.GetByIdAsync(id);

            if (listOfValue == null)
                throw new BusinessLogicException($"Setting with id {id} does not exist.");

            if (listOfValue.Type != WebSettingType)
                throw new BusinessLogicException($"Setting with id {id} is not a web setting.");

            listOfValue.DisplayValue = setting.DisplayValue ? "true" : "false";

            await _listOfValueDataService.SaveAsync(listOfValue, currentlyLoggedInUserId);

            return new SettingDto
            {
                RowID = listOfValue.RowID.Value,
                LIC = listOfValue.LIC,
                DisplayValue = Convert.ToBoolean(listOfValue.DisplayValue)
            };
        }
    }
}
