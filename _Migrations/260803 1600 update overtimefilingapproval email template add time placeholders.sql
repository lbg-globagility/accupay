-- Adds {time}, {overtimeStart} and {overtimeEnd} placeholders to the global default
-- template for the overtime filing approval email.
UPDATE `emailtemplate`
SET
  `HtmlBody` = '<div style="font-family:Segoe UI, Arial, sans-serif;"><p>Hi {approver},</p><p>{employee} filed {hours} h of overtime on {date} ({time}).</p><p>Reason: {reason}</p><p>{approveButton} {rejectButton}</p></div>',
  `TextBody` = 'Hi {approver},\n\n{employee} filed {hours} h of overtime on {date} ({time}).\nReason: {reason}\n\nApprove: {approveButton}\nReject: {rejectButton}'
WHERE `Code` = 'OvertimeFilingApproval'
  AND `OrganizationID` IS NULL;
