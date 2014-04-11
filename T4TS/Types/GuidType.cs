namespace T4TS
{
    public class GuidType : TypescriptType
    {
        public GuidType(bool isKnockoutObservable = false)
            : base(isKnockoutObservable)
        {
        }

        public override string Name
        {
            get
            {
                if (this.isKnockoutObservable)
                    return string.Format("{0}<string>", KnockoutObservable);

                return "string";
            }
        }
    }
}
