using QueryPressure.Arguments;

namespace QueryPressure.Interfaces;

public interface ICreator<out T>
{
    string Type { get; }
    T Create(ArgumentsSection arguments);
}
