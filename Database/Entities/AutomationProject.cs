using System;
using IEnge.Database.Entities;

namespace IEnge.Controllers.Api
{
    public class AutomationProject: DbMotherObj
    {
        public string Name { get; set; }
        public string Desc { get; set; }
    }
}
