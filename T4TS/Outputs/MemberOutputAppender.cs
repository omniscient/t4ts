﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4TS
{
    public class MemberOutputAppender : OutputAppender<TypeScriptInterfaceMember>
    {
        public MemberOutputAppender(StringBuilder output, int baseIndentation, Settings settings)
            : base(output, baseIndentation, settings)
        {
        }

        public override void AppendOutput(TypeScriptInterfaceMember member)
        {
            AppendIndendation();

            bool isOptional = member.Optional || (member.Type is NullableType);
            string type = member.Type.ToString();

            Output.AppendFormat("{0}{1}: {2}",
                member.Name,
                (isOptional ? "?" : ""),
                type
            );

            Output.AppendLine(";");
        }
    }
}
