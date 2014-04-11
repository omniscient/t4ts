namespace T4TS
{
    public class DateTimeType : TypescriptType
    {
        public DateTimeType(bool isKnockoutObservable = false)
            : base(isKnockoutObservable)
        {
        }

        public override string Name
        {
            get
            {
                if (this.isKnockoutObservable)
                    return string.Format("{0}<Date>", KnockoutObservable);

                return "Date";
            }
        }
    }
}
