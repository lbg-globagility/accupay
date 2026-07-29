using AccuPay.Core.Entities;
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

        public SettingService(IListOfValueRepository listOfValueRepository)
        {
            _listOfValueRepository = listOfValueRepository;
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
    }
}
