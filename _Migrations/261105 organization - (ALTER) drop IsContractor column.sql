-- Contractor status now comes from project_employees -> contractor_project -> contractor,
-- per employee and pay period, so the organization-wide flag is no longer read by anything.
-- This column is the only record of which organizations were contractors. Before running this,
-- list them and assign their employees to a contractor project:
--     SELECT RowID, Name FROM organization WHERE IsContractor = 1;
-- Install the updated PAYROLLSUMMARY2 procedure as well.
ALTER TABLE `organization`
    DROP COLUMN `IsContractor`;
