using AccuPay.Data.Entities;
using System;
using System.Collections.Generic;

namespace AccuPay.Data.Services.Imports.OfficialBusiness
{
    public class OfficialBusinessImportModel : OfficialBusinessModel
    {
        private OfficialBusinessRowRecord _parsedRecord;
        private Employee _employee;
        private readonly bool _noEmployee;
        private Entities.OfficialBusiness _officialBusiness;
        private bool _noStartDate;
        private bool _noStartTime;
        private bool _noEndTime;
        private readonly bool _officialBusinessNotYetExists;

        public OfficialBusinessImportModel(OfficialBusinessRowRecord parsedRecord, Employee employee, Entities.OfficialBusiness officialBusiness)
        {
            _parsedRecord = parsedRecord;

            _employee = employee;
            _noEmployee = employee == null;

            _officialBusiness = officialBusiness;
            _officialBusinessNotYetExists = officialBusiness == null;

            ApplyData(parsedRecord);
        }

        private void ApplyData(OfficialBusinessRowRecord parsedRecord)
        {
            EmployeeNo = parsedRecord.EmployeeNo;

            if (!_noEmployee)
            {
                EmployeeID = _employee.RowID;
                FullName = _employee.FullNameLastNameFirst;
            }

            _noStartDate = !parsedRecord.StartDate.HasValue;
            if (!_noStartDate) StartDate = parsedRecord.StartDate;

            _noStartTime = !parsedRecord.StartTime.HasValue;
            if (!_noStartTime) StartTime = parsedRecord.StartTime;

            _noEndTime = !parsedRecord.EndTime.HasValue;
            if (!_noEndTime) EndTime = parsedRecord.EndTime;
        }

        public bool InvalidToSave
        {
            get
            {
                return _noEmployee ||
                    _noStartDate ||
                    _noStartTime ||
                    _noEndTime ||
                    !_officialBusinessNotYetExists;
            }
        }

        internal void DescribeError()
        {
            List<string> errors = new List<string>();

            if (_noEmployee) errors.Add("Employee doesn't exists");
            if (_noStartDate) errors.Add("Invalid Effective start date");
            if (_noStartTime) errors.Add("Invalid Effective Start Time");
            if (_noEndTime) errors.Add("Invalid Effective End Time");
            if (!_officialBusinessNotYetExists) errors.Add("Official Business already exists");

            Remarks = string.Join("; ", errors.ToArray());
        }

        public string Remarks { get; set; }
    }
}