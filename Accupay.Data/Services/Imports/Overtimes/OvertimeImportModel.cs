using AccuPay.Data.Entities;
using System;
using System.Collections.Generic;

namespace AccuPay.Data.Services.Imports.Overtimes
{
    public class OvertimeImportModel : OvertimeModel
    {
        private OvertimeRowRecord _parsedRecord;
        private Employee _employee;
        private readonly bool _noEmployee;
        private Overtime _overtime;
        private readonly bool _overtimeNotYetExists;
        private bool _noStartDate;
        private bool _noStartTime;
        private bool _noEndTime;

        public OvertimeImportModel(OvertimeRowRecord parsedRecord, Employee employee, Overtime overtime)
        {
            _parsedRecord = parsedRecord;
            _employee = employee;
            _noEmployee = _employee == null;

            _overtime = overtime;
            _overtimeNotYetExists = _overtime == null;

            ApplyData(parsedRecord);
        }

        private void ApplyData(OvertimeRowRecord parsedRecord)
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

        public bool IsOvertimeNotYetExists
        {
            get
            {
                return _overtimeNotYetExists;
            }
        }

        public bool InvalidToSave
        {
            get
            {
                return _noEmployee ||
                    _noStartDate ||
                    _noStartTime ||
                    _noEndTime ||
                    !_overtimeNotYetExists;
            }
        }

        internal void DescribeError()
        {
            List<string> errors = new List<string>();

            if (_noEndTime) errors.Add("Employee doesn't exists");
            if (_noStartDate) errors.Add("Invalid Effective start date");
            if (_noStartTime) errors.Add("Invalid Effective Start Time");
            if (_noEndTime) errors.Add("Invalid Effective End Time");
            if (!_overtimeNotYetExists) errors.Add("Overtime already exists");

            Remarks = string.Join("; ", errors.ToArray());
        }

        public string Remarks { get; set; }
    }
}