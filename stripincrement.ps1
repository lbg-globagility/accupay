param([string]$filename = "");

$content = Get-Content $filename
$content = $content -replace 'AUTO_INCREMENT=[0-9]* ', ''

$content | Out-File -FilePath $filename -Encoding utf8 -Force;
