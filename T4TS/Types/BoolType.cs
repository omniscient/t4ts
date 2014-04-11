using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4TS
{
    public class BoolType : TypescriptType
    {
        private Version version;

        public BoolType(bool isKnockoutObservable = false, Version version = null)
            : base(isKnockoutObservable)
        {
            this.version = version;
        }

        public override string Name
        {
            get
            {
                if (this.isKnockoutObservable)
                    return string.Format("{0}<boolean>", KnockoutObservable);
                
                if (this.version != null && this.version < new Version(0, 9, 0))
                    return "bool";
                
                return "boolean";
            }
        }
    }
}
