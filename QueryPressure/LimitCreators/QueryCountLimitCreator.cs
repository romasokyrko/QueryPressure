using QueryPressure.Arguments;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.Limits;
using QueryPressure.Interfaces;

namespace QueryPressure.LimitCreators
{
    public class QueryCountLimitCreator : ICreator<ILimit>
    {
        public string Type => "queryCount";

        public ILimit Create(ArgumentsSection argumentsSection)
        {
            return new QueryCountLimit(argumentsSection.ExtractIntArgumentOrThrow("limit"));
        }
    }
}
