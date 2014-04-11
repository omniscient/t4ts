using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4TS
{
    public class ArrayType: TypescriptType
    {
        public ArrayType(bool knockoutObservable = false)
            : base(knockoutObservable)
        {
        }

        public TypescriptType ElementType { get; set; }

        public override string ToString()
        {
            if (this.isKnockoutObservable)
                return string.Format("KnockoutObservableArray<{0}>", ElementType.Name);
            
            return ElementType + "[]";
        }
    }
}
