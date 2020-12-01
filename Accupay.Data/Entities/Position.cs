using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("position")]
    public class Position : BaseOrganizationalEntity
    {
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
