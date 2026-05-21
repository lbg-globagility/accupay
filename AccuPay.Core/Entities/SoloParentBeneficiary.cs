using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccuPay.Core.Entities
{
    [Table("soloparentbeneficiary")]
    public partial class SoloParentBeneficiary : OrganizationalEntity
    {
        public int EmployeeId { get; set; }

        public bool HasValidityPassed { get; set; } = false;

        public byte[] Attachment { get; set; } = null;

        public string AttachmentFileName { get; set; } = null;

    }

    public partial class SoloParentBeneficiary
    {
        public static decimal LEAVE_HOURS =
            7 // daily working hours
            * 8; // number of working hours in a day

        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }

        public bool HasSingleParentID => !string.IsNullOrEmpty(AttachmentFileName)
            && (Attachment != null && Attachment.Length > 0);

        private SoloParentBeneficiary()
        {
        }

        public SoloParentBeneficiary(int userId,
            int orgId,
            int employeeId,
            string attachmentFileName = null)
        {
            OrganizationID= orgId;
            EmployeeId = employeeId;

            if (!string.IsNullOrEmpty(attachmentFileName))
                AttachFile(attachmentFileName);

            CreatedBy = userId;
        }

        private async void AttachFile(string attachmentFileName)
        {
            await AttachFileAsync(attachmentFileName);
        }

        public async Task AttachFileAsync(string attachmentFileName)
        {
            await ReadAllBytesAsync(attachmentFileName)
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        Attachment = task.Result;
                        AttachmentFileName = attachmentFileName;
                    }
                });
        }

        public void DetachFile()
        {
            Attachment = null;
            AttachmentFileName = null;
        }   

        public string FileNameAndExtensionOnly => Path.GetFileName(AttachmentFileName);

        public static SoloParentBeneficiary Create(int userId,
            int orgId,
            int employeeId,
            string attachmentFileName = null)
            => new SoloParentBeneficiary(userId: userId, orgId: orgId, employeeId: employeeId, attachmentFileName: attachmentFileName);

        private async Task<byte[]> ReadAllBytesAsync(string filePath)
        {
            // The 'true' at the end enables Asynchronous I/O at the OS level
            using (var sourceStream = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 4096,
                useAsync: true))
            {
                var buffer = new byte[sourceStream.Length];
                await sourceStream.ReadAsync(buffer, 0, (int)sourceStream.Length);
                return buffer;
            }
        }

        public async Task ViewFileAsync()
        {
            if (!HasSingleParentID)
            {
                MessageBox.Show("No file fetched.", "AccuPay", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var tempFileName = Path.Combine(Path.GetTempPath(), Path.GetFileName(AttachmentFileName));

            using (FileStream fs = new FileStream(tempFileName,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 4096,
                useAsync: true))
            {
                await fs.WriteAsync(Attachment, 0, Attachment.Length);
            }

            try
            {
                var startInfo = new ProcessStartInfo(tempFileName)
                {
                    UseShellExecute = true // Required in .NET Core/.NET 5+ to open files with default apps
                };

                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not open the file. {ex?.Message}", "AccuPay", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
