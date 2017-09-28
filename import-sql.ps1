$pwd = Read-Host 'password';
$database = Read-Host 'database';
$server = Read-Host 'server';

Get-ChildItem ./goldwingspayrolldb |
Foreach-Object {
    $filename = $_.FullName;

    Get-Content $filename |
        mysql --user=root --password=$pwd --database=$database --comments;

    Write-Output $filename;
}
