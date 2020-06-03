using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("position")]
    public class Position : BaseEntity
    {
        public int? OrganizationID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public int? ParentPositionID { get; set; }

        public int? DivisionID { get; set; }

        public int? JobLevelID { get; set; }

        [Column("PositionName")]
        public string Name { get; set; }

        public int LevelNumber { get; set; }

        [ForeignKey("DivisionID")]
        public virtual Division Division { get; set; }

        [ForeignKey("JobLevelID")]
        public virtual JobLevel JobLevel { get; set; }
    }
}