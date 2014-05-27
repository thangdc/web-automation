using System;
using System.Collections.Generic;
using System.Text;

namespace Automation
{
    class Website
    {

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        private string _Url;
        public string Url
        {
            get { return _Url; }
            set { _Url = value; }
        }

        private string _Categories;
        public string Categories
        {
            get { return _Categories; }
            set { _Categories = value; }
        }

        public string Parent
        {
            get;
            set;
        }

        public bool IsValid
        {
            get;
            set;
        }

        public bool IsRead
        {
            get;
            set;
        }

        public bool IsInternal
        {
            get;
            set;
        }

        private int? _Status;
        public int? Status
        {
            get { return _Status; }
            set 
            {
                if (_Status != null)
                    _Status = value;
                else
                    _Status = 0;
            }
        }

        public Website()
        {

        }
    }
}
