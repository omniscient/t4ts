using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4TS
{
    public class NumberType : TypescriptType
    {
        public NumberType(bool isKnockoutObservable = false)
            : base(isKnockoutObservable)
        {
        }

        public override string Name
        {
            get
            {
                if (this.isKnockoutObservable)
                    return string.Format("{0}<number>", KnockoutObservable);

                return "number";
            }
        }
    }
}
