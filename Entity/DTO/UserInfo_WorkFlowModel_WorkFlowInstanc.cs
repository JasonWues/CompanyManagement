using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTO
{
    public class UserInfo_ConsumableInfo_WorkFlowModel_WorkFlowInstanc
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public string ModelTitle { get; set; }

        public string ConsumableName { get; set; }
        public int OutNum { get; set; }

        public string Status { get; set; }
        public string CreateTime { get; set; }
    }
}
