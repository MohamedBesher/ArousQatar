using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Core.Dtos
{
    public class ContactInformationDto
    {
        public int Id { get; set; }
        public string Contact { get; set; }
        public int ContactTypeId { get; set; }
        public string IconName { get; set; }
        public string ContactTypeName { get; set; }
        public int OverallCount { get; set; }

    }
}
