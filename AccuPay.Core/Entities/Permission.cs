﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("permission")]
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public bool ForDesktopOnly { get; set; }
    }
}