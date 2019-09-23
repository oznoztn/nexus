using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Tools
{
    public static class Toolkit
    {
        public static string ParseToJsArray(IEnumerable<string> noteTagDtos)
        {
            StringBuilder sbuilder = new StringBuilder();
            var tags = noteTagDtos.ToArray();

            sbuilder.Append("[");
            for (int i = 0; i < tags.Length; i++)
            {
                var tag = tags[i];

                sbuilder.Append("'");
                sbuilder.Append(tag);
                sbuilder.Append("'");

                if (i != tags.Length - 1)
                {
                    sbuilder.Append(",");
                }
            }
            sbuilder.Append("]");

            return sbuilder.ToString();
        }
    }
}
