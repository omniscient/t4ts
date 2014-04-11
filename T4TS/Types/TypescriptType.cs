using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4TS
{
    public class TypescriptType
    {
        protected const string KnockoutObservable = "KnockoutObservable";
        
        protected bool isKnockoutObservable;

        public TypescriptType(bool isKnockoutObservable = false)
        {
            this.isKnockoutObservable = isKnockoutObservable;
        }

        public virtual string Name
        {
            get
            {
                return "any";
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
