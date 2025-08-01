; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "AccuPay"
#define MyAppVersion "1.37.2"
#define MyAppPublisher "Globagility, Inc."
#define MyAppURL "http://www.globagilityinc.com/"
#define MyAppExeName "AccuPay.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{1F45D0F9-8D6A-46A3-BC3C-1DE2B354D393}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} v{#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\GLOBAGILITY\AccuPay
DisableProgramGroupPage=yes
OutputBaseFilename=accupaysetup-v{#MyAppVersion}
Compression=lzma
UsePreviousAppDir=yes
SolidCompression=yes
VersionInfoVersion={#MyAppVersion}

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: ".\AccuPay\bin\Debug\AccuPay.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\AccuPay.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\AccuPay.CrystalReports.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\AccuPay.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\AccuPay.Infrastructure.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\AccuPay.Utilities.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Aga.Controls.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\AutoMapper.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Castle.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\CollapsibleGroupBox.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\CrystalDecisions.CrystalReports.Engine.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\CrystalDecisions.ReportAppServer.ClientDoc.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\CrystalDecisions.ReportAppServer.CommLayer.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\CrystalDecisions.ReportAppServer.CommonControls.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\CrystalDecisions.ReportAppServer.CommonObjectModel.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\CrystalDecisions.ReportAppServer.Controllers.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\CrystalDecisions.ReportAppServer.CubeDefModel.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\CrystalDecisions.ReportAppServer.DataDefModel.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\CrystalDecisions.ReportAppServer.DataSetConversion.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\CrystalDecisions.ReportAppServer.ObjectFactory.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\CrystalDecisions.ReportAppServer.Prompting.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\CrystalDecisions.ReportAppServer.ReportDefModel.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\CrystalDecisions.ReportAppServer.XmlSerialize.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\CrystalDecisions.ReportSource.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\CrystalDecisions.Shared.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\CrystalDecisions.Windows.Forms.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\DevComponents.DotNetBar2.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\EPPlus.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\EWSoftware.ListControls.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Femiani.Forms.UI.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\FlashControlV71.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\log4net.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\MessageBoxManager.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\MetroFramework.Design.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\MetroFramework.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\MetroFramework.Fonts.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.AspNetCore.Authentication.Abstractions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.AspNetCore.Authentication.Cookies.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.AspNetCore.Authentication.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.AspNetCore.Authentication.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.AspNetCore.Cryptography.Internal.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.AspNetCore.Cryptography.KeyDerivation.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.AspNetCore.DataProtection.Abstractions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.AspNetCore.DataProtection.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.AspNetCore.Hosting.Abstractions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.AspNetCore.Hosting.Server.Abstractions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.AspNetCore.Http.Abstractions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.AspNetCore.Http.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.AspNetCore.Http.Extensions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.AspNetCore.Http.Features.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.AspNetCore.Identity.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.AspNetCore.Identity.EntityFrameworkCore.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.AspNetCore.WebUtilities.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.EntityFrameworkCore.Abstractions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.EntityFrameworkCore.Design.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.EntityFrameworkCore.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.EntityFrameworkCore.Proxies.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.EntityFrameworkCore.Relational.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.Extensions.Caching.Abstractions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.Extensions.Caching.Memory.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.Extensions.Configuration.Abstractions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.Extensions.Configuration.Binder.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.Extensions.Configuration.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.Extensions.DependencyInjection.Abstractions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.Extensions.DependencyInjection.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.Extensions.FileProviders.Abstractions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.Extensions.Hosting.Abstractions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.Extensions.Identity.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.Extensions.Identity.Stores.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.Extensions.Logging.Abstractions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.Extensions.Logging.Configuration.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.Extensions.Logging.Console.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.Extensions.Logging.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.Extensions.ObjectPool.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.Extensions.Options.ConfigurationExtensions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.Extensions.Options.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.Extensions.Primitives.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.Extensions.WebEncoders.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.Net.Http.Headers.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.Win32.Primitives.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Microsoft.Win32.Registry.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\MySqlConnector.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\netstandard.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\OCRTools.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\PdfSharp.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Pomelo.EntityFrameworkCore.MySql.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Pomelo.JsonObject.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\Remotion.Linq.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\ShockwaveFlashObjects.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\stdole.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.AppContext.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Buffers.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Collections.Concurrent.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Collections.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Collections.Immutable.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Collections.NonGeneric.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Collections.Specialized.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.ComponentModel.Annotations.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.ComponentModel.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.ComponentModel.EventBasedAsync.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.ComponentModel.Primitives.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.ComponentModel.TypeConverter.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Console.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Data.Common.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Diagnostics.Contracts.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Diagnostics.Debug.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Diagnostics.DiagnosticSource.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Diagnostics.FileVersionInfo.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Diagnostics.Process.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Diagnostics.StackTrace.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Diagnostics.TextWriterTraceListener.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Diagnostics.Tools.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Diagnostics.TraceSource.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Diagnostics.Tracing.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Drawing.Primitives.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Dynamic.Runtime.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Globalization.Calendars.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Globalization.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Globalization.Extensions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Interactive.Async.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.IO.Compression.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.IO.Compression.ZipFile.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.IO.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.IO.FileSystem.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.IO.FileSystem.DriveInfo.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.IO.FileSystem.Primitives.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.IO.FileSystem.Watcher.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.IO.IsolatedStorage.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.IO.MemoryMappedFiles.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.IO.Pipes.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.IO.UnmanagedMemoryStream.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Linq.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Linq.Expressions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Linq.Parallel.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Linq.Queryable.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Memory.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Net.Http.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Net.NameResolution.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Net.NetworkInformation.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Net.Ping.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Net.Primitives.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Net.Requests.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Net.Security.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Net.Sockets.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Net.WebHeaderCollection.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Net.WebSockets.Client.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Net.WebSockets.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Numerics.Vectors.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.ObjectModel.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Reflection.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Reflection.Extensions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Reflection.Primitives.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Resources.Reader.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Resources.ResourceManager.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Resources.Writer.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Runtime.CompilerServices.Unsafe.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Runtime.CompilerServices.VisualC.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Runtime.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Runtime.Extensions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Runtime.Handles.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Runtime.InteropServices.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Runtime.InteropServices.RuntimeInformation.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Runtime.Numerics.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Runtime.Serialization.Formatters.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Runtime.Serialization.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Runtime.Serialization.Primitives.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Runtime.Serialization.Xml.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Security.AccessControl.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Security.Claims.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Security.Cryptography.Algorithms.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Security.Cryptography.Csp.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Security.Cryptography.Encoding.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Security.Cryptography.Primitives.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Security.Cryptography.X509Certificates.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Security.Cryptography.Xml.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Security.Permissions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Security.Principal.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Security.Principal.Windows.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Security.SecureString.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Text.Encoding.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Text.Encoding.Extensions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Text.Encodings.Web.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Text.RegularExpressions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Threading.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Threading.Overlapped.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Threading.Tasks.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Threading.Tasks.Extensions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Threading.Tasks.Parallel.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Threading.Thread.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Threading.ThreadPool.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Threading.Timer.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.ValueTuple.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Xml.ReaderWriter.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Xml.XDocument.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Xml.XmlDocument.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Xml.XmlSerializer.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\log4.config"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\bin\Debug\System.Xml.XPath.dll"; DestDir: "{app}"; Flags: ignoreversion

Source: ".\AccuPay\Resources\*.xlsx"; DestDir: "{app}\Resources"; Flags: ignoreversion
Source: ".\AccuPay\ImportTemplates\*.xlsx"; DestDir: "{app}\ImportTemplates"; Flags: ignoreversion
Source: ".\AccuPay\Resources\SourceSansPro-Regular.ttf"; DestDir: "{fonts}"; FontInstall: "Source Sans Pro"; Flags: onlyifdoesntexist uninsneveruninstall

Source: ".\AccuPay\BankFileHeaderData.dat"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\AccuPay\BankTemplates\*.xlsm"; DestDir: "{app}\BankTemplates"; Flags: ignoreversion
Source: ".\AccuPay\BankTemplates\*.xlsx"; DestDir: "{app}\BankTemplates"; Flags: ignoreversion
Source: ".\AccuPay\BankTemplates\SecurityBank\*.xlsm"; DestDir: "{app}\BankTemplates\SecurityBank"; Flags: ignoreversion

; Source: ".\AccuPay\bin\Debug\Core Forms\*.rpt"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; Source: ".\AccuPay\bin\Debug\Core Forms\rpt\*.rpt"; DestDir: "{app}\Core Forms\rpt"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Code]
var
  Page: TInputQueryWizardPage;
  UserName, UserCompany: String;

function GetHost(Param: string): string;
begin
  Result := Page.Values[0];
end;

function GetDatabase(Param: string): string;
begin
  Result := Page.Values[1];
end;

procedure InitializeWizard();
var databaseName, serverName: String;
begin
  Page := CreateInputQueryPage(wpWelcome,
  'Database Information', 'Database to connect to',
  'Please specify the database host and name, then click Next.');

  { Add items (False means it's not a password edit) }
  Page.Add('Host:', False);
  Page.Add('Database:', False);

  { Set initial values (optional) }
  begin    
    if RegQueryStringValue(HKEY_LOCAL_MACHINE, 'SOFTWARE\Globagility\DBConn\GoldWings',
       'server', serverName) then
    begin
    end;
  end;
  Page.Values[0] := ExpandConstant(serverName);

  begin    
    if RegQueryStringValue(HKEY_LOCAL_MACHINE, 'SOFTWARE\Globagility\DBConn\GoldWings',
       'database', databaseName) then
    begin
    end;
  end;
  Page.Values[1] := ExpandConstant(databaseName);
end;

[Registry]

Root: HKLM; Subkey: "SOFTWARE\Globagility";
Root: HKLM; Subkey: "SOFTWARE\Globagility\DBConn";

Root: HKLM; Subkey: "SOFTWARE\Globagility\DBConn\GoldWings";  Flags: createvalueifdoesntexist;	ValueType: string; ValueName: "server"; ValueData: "{code:GetHost}"
Root: HKLM; Subkey: "SOFTWARE\Globagility\DBConn\GoldWings";  Flags: createvalueifdoesntexist;	ValueType: string; ValueName: "database"; ValueData: "{code:GetDatabase}"
Root: HKLM; Subkey: "SOFTWARE\Globagility\DBConn\GoldWings";  Flags: createvalueifdoesntexist;	ValueType: string; ValueName: "user id"; ValueData: "root"
Root: HKLM; Subkey: "SOFTWARE\Globagility\DBConn\GoldWings";  Flags: createvalueifdoesntexist;	ValueType: string; ValueName: "password"; ValueData: "globagility"
Root: HKLM; Subkey: "SOFTWARE\Globagility\DBConn\GoldWings";  ValueType: string; ValueName: "apppath"; ValueData: "{app}"

Root: "HKLM"; Subkey: "SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers\";  Flags: createvalueifdoesntexist;	ValueType: String; ValueName: "{app}\{#MyAppExeName}"; ValueData: "RUNASADMIN";

[Icons]
Name: "{commonprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: runascurrentuser nowait postinstall skipifsilent

