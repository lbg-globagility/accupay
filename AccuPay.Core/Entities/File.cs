using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("file")]
    public class File
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }

        public int CreatedById { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public string Key { get; set; }

        public string Filename { get; set; }

        public string Path { get; set; }

        public string MediaType { get; set; }

        public long Size { get; set; }

        private File()
        {
        }

        public File(string key, string path, string filename, string mediaType, long size)
        {
            Key = key;
            Filename = filename;
            Path = path;
            MediaType = mediaType;
            Size = size;
        }

        public File(string key, string location, IFormFile file)
            : this(key, location, file.FileName, file.ContentType, file.Length)
        {
        }
    }
}