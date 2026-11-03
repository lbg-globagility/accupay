SET @id=(SELECT RowID FROM listofval WHERE `Type`='ReportProviders' AND DisplayValue='RemittanceReportProvider' LIMIT 1);

INSERT INTO `listofval` (`RowID`, `OrganizationID`, `Created`, `CreatedBy`, `LastUpd`, `LastUpdBy`, `DisplayValue`, `LIC`, `Type`, `ParentLIC`, `Active`, `Description`, `OrderBy`) VALUES (@id, NULL, CURRENT_TIMESTAMP(), 4, CURRENT_TIMESTAMP(), 4, 'RemittanceReportProvider', 'RemittanceReportProvider', 'ReportProviders', NULL, 'Yes', NULL, 4) ON DUPLICATE KEY UPDATE LastUpd=CURRENT_TIMESTAMP();
