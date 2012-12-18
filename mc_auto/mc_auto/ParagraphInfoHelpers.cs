using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionMethods;

namespace mc_auto
{
    internal partial class ParagraphInfo
    {
        private class Strategy
        {
            internal static readonly Strategy Default = new Strategy(
                st: StdStart
                , en: StdEnd
                , isc: StdIsChild
                , gl: StdGetLevel
            );
            public Strategy(Starter st, Ender en, IsChilder isc, GetLeveler gl)
            {
                DoStart = st;
                DoEnd = en;
                DoIsChild = isc;
                DoGetLevel = gl;
            }
            internal readonly Starter DoStart;
            internal readonly Ender DoEnd;
            internal readonly IsChilder DoIsChild;
            internal readonly GetLeveler DoGetLevel;

        }
        private class StrategyKey
        {
            private static readonly IDictionary<string, bool> paragraphInfos = new Dictionary<string, bool>()
            {
                {MC_TECH_STYLE, true}
                ,{"mc_example_text", true}
            };


            private int hash;

            public StrategyKey(WrapperType wt)
                : this(wt, 0, string.Empty, string.Empty)
            {
            }
            public StrategyKey(int outlineLevel)
                : this(WrapperType.None, outlineLevel, string.Empty, string.Empty)
            {
            }
            public StrategyKey(string style, string previousStyle)
                : this(WrapperType.None, 0, style, previousStyle)
            {

            }
            public StrategyKey(WrapperType wt, int outlineLevel, string style, string previousStyle)
            {
                hash = Hash(wt) > 0 ? Hash(wt)
                  : Hash(outlineLevel) > 0 ? Hash(outlineLevel)
                  : Hash(style, previousStyle);
            }

            private int Hash(string style, string previousStyle)
            {
                int st = (paragraphInfos.GetValueOrDefault(style, false) ? 0x1 : 0x0) << 3;
                int st2 = (paragraphInfos.GetValueOrDefault(previousStyle, false) ? 0x1 : 0x0) << 4;
                return st | st2;
            }

            private int Hash(int outlineLevel)
            {
                return (outlineLevel < BODY_LEVEL && outlineLevel > 0 ? 0x1 : 0x0) << 2;
            }

            private int Hash(WrapperType wt)
            {
                return wt == WrapperType.EndMarker ? 0x01 : wt == WrapperType.Root ? (0x1 << 1) : 0x00;
            }
            // override object.Equals
            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }

                return hash == (obj as StrategyKey).hash;
            }

            // override object.GetHashCode
            public override int GetHashCode()
            {
                return hash;
            }
        }

    }
}
