using EnvDTE;
using System.Collections.Generic;
using System.Linq;

namespace T4TS
{
    public class TypeContext
    {
        public Settings Settings { get; private set; }

        public TypeContext(Settings settings)
        {
            this.Settings = settings;
        }

        private static readonly string[] genericCollectionTypeStarts = new string[] {
            "System.Collections.Generic.List<",
            "System.Collections.Generic.IList<",
            "System.Collections.Generic.ICollection<",
			"System.Data.Objects.DataClasses.EntityCollection<"
        };

        private static readonly string nullableTypeStart = "System.Nullable<";

        /// <summary>
        /// Lookup table for "interface types", ie. non-builtin types (typically classes or unknown types). Keyed on the FullName of the type.
        /// </summary>
        private Dictionary<string, InterfaceType> interfaceTypes = new Dictionary<string, InterfaceType>();

        public void AddInterfaceType(string typeFullName, InterfaceType interfaceType)
        {
            interfaceTypes.Add(typeFullName, interfaceType);
        }

        public bool TryGetInterfaceType(string typeFullName, out InterfaceType interfaceType)
        {
            return interfaceTypes.TryGetValue(typeFullName, out interfaceType);
        }

        public bool ContainsInterfaceType(string typeFullName)
        {
            return interfaceTypes.ContainsKey(typeFullName);
        }

        public TypescriptType GetTypeScriptType(CodeTypeRef codeType)
        {
            switch (codeType.TypeKind)
            {
                case vsCMTypeRef.vsCMTypeRefChar:
                case vsCMTypeRef.vsCMTypeRefString:
                    return new StringType(this.Settings.KnockoutObservable);

                case vsCMTypeRef.vsCMTypeRefBool:
                    return new BoolType(this.Settings.KnockoutObservable, this.Settings.CompatibilityVersion);

                case vsCMTypeRef.vsCMTypeRefByte:
                case vsCMTypeRef.vsCMTypeRefDouble:
                case vsCMTypeRef.vsCMTypeRefInt:
                case vsCMTypeRef.vsCMTypeRefShort:
                case vsCMTypeRef.vsCMTypeRefFloat:
                case vsCMTypeRef.vsCMTypeRefLong:
                case vsCMTypeRef.vsCMTypeRefDecimal:
                    return new NumberType(this.Settings.KnockoutObservable);

                default:
                    return TryResolveType(codeType);
            }
        }

        private TypescriptType TryResolveType(CodeTypeRef codeType)
        {
            if (codeType.TypeKind == vsCMTypeRef.vsCMTypeRefArray)
            {
                return new ArrayType(this.Settings.KnockoutObservable)
                {
                    ElementType = GetTypeScriptType(codeType.ElementType)
                };
            }

            return GetTypeScriptType(codeType.AsFullName);
        }

        private ArrayType TryResolveEnumerableType(string typeFullName)
        {
            return new ArrayType(this.Settings.KnockoutObservable)
            {
                ElementType = GetTypeScriptType(typeFullName)
            };
        }

        public TypescriptType GetTypeScriptType(string typeFullName)
        {
            InterfaceType interfaceType;
            if (interfaceTypes.TryGetValue(typeFullName, out interfaceType))
                return interfaceType;

            if (IsGenericEnumerable(typeFullName))
            {
                return new ArrayType(this.Settings.KnockoutObservable)
                {
                    ElementType = GetTypeScriptType(UnwrapGenericType(typeFullName))
                };
            }
            else if (IsNullable(typeFullName))
            {
                return new NullableType
                {
                    WrappedType = GetTypeScriptType(UnwrapGenericType(typeFullName))
                };
            }

            switch (typeFullName)
            {
				case "System.Guid":
                    return new GuidType(this.Settings.KnockoutObservable);
                case "System.Boolean":
                    return new BoolType(this.Settings.KnockoutObservable);
                case "System.Double":
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                case "System.UInt16":
                case "System.UInt32":
                case "System.UInt64":
                case "System.Decimal":
                case "System.Byte":
                case "System.SByte":
                case "System.Single":
                    return new NumberType(this.Settings.KnockoutObservable);

                case "System.String":
					return new StringType(this.Settings.KnockoutObservable);
                case "System.DateTime":
                    if (Settings.UseNativeDates)
                        return new DateTimeType(this.Settings.KnockoutObservable);

                    return new StringType(this.Settings.KnockoutObservable);
					
                default:
                    return new TypescriptType(this.Settings.KnockoutObservable);
            }
        }

        private bool IsNullable(string typeFullName)
        {
            return typeFullName.StartsWith(nullableTypeStart);
        }

        public string UnwrapGenericType(string typeFullName)
        {
            int firstIndex = typeFullName.IndexOf('<');
            return typeFullName.Substring(firstIndex + 1, typeFullName.Length - firstIndex - 2);
        }

        public bool IsGenericEnumerable(string typeFullName)
        {
            return genericCollectionTypeStarts.Any(t => typeFullName.StartsWith(t));
        }
    }
}
