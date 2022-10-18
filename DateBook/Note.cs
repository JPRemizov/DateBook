﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DateBook
{
    public class Note
    {

        private string _name;
        private string _description;
        private string _dateComp;
        private DateTime _dateNow;

        public Note(string name, string description, string dateComp, DateTime dateNow)
        {
            this._name = name; this._description = description; this._dateComp = dateComp; this._dateNow = dateNow;
        }
        public Note() { }
        public string name
        {
            get { return _name; }
            set { _name = value; }

        }
        public string description
        {
            get { return _description; }
            set { _description = value; }
        }
        public string dateComp
        {
            get { return _dateComp; }
            set { _dateComp = value; }
        }
        public DateTime dateNow
        {
            get { return _dateNow; }
            set { _dateNow = value; }
        }

    }
}
