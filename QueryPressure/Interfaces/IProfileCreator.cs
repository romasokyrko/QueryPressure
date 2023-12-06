using QueryPressure.Arguments;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.Interfaces;

interface IProfileCreator
{
    string ProfileTypeName { get; }
    IProfile Create(ProflieArguments arguments);
}
