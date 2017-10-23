$server = Read-Host 'server';
$database = Read-Host 'database';
$pwd = Read-Host 'password' -AsSecureString;

$pwd = [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($pwd));

Get-ChildItem ./goldwingspayrolldb |
Foreach-Object {
    $filename = $_.FullName;

    If (!$server) {
        Get-Content $filename |
            mysql --user=root --password=$pwd --database=$database --comments;
    }
    Else {
        Get-Content $filename |
            mysql --user=root --host=$server --password=$pwd --database=$database --comments;
    }

    Write-Output $filename;
}
