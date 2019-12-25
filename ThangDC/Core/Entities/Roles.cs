using System;
using System.Collections.Generic;
using System.Text;

namespace ThangDC.Core.Entities
{
    public class Roles
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
    }
}
