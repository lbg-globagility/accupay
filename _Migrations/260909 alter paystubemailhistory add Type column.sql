-- Add Type column to paystubemailhistory so the sent-email history records which
-- report (Payslip, DailyAttendanceReport, AccessOffshoringPayslip, ...) was actually emailed.
-- Existing rows predate this and cannot be reliably backfilled, so they are left NULL.
ALTER TABLE `paystubemailhistory`
    ADD COLUMN `Type` VARCHAR(255) NULL DEFAULT NULL AFTER `IsActual`;
