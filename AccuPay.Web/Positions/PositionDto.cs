using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Positions
{
    public class PositionDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int DivisionId { get; set; }

        public string DivisionName { get; set; }
    }
}
