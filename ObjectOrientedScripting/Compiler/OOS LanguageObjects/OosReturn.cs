﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.OOS_LanguageObjects
{
    public class OosReturn : BaseLangObject
    {
        public BaseLangObject Expression { get { return this.Children[0]; } set { this.Children[0] = value; if(value != null) value.setParent(this); } }
        private List<BaseLangObject> Instructions { get { return this.Children.GetRange(1, this.Children.Count - 1); } }

        public OosReturn()
        {
            this.addChild(null);
        }
    }
}
