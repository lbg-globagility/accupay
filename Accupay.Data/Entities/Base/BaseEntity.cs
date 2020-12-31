using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int? RowID { get; set; }

        /// <summary>
        /// Checks if the entity is new (not yet in the database), by checking its RowID if it is less than or equal to 0.
        /// </summary>
        public bool IsNewEntity => CheckIfNewEntity(RowID);

        /// <summary>
        /// Checks if the entity is new (not yet in the database), by checking its RowID if it is less than or equal to 0.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool CheckIfNewEntity(int? id)
        {
            // sometimes it's not int.MinValue
            return id == null || id <= 0;
        }
    }
}
