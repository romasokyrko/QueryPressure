﻿using QueryPressure.Arguments;
using QueryPressure.Core.Interfaces;
using QueryPressure.Interfaces;

namespace QueryPressure.Factories;

public class SettingsFactory<T> where T : ISetting
{
    private readonly IDictionary<string, ICreator<T>> _creators;
    private readonly string _settingType;

    public SettingsFactory(string settingType, IEnumerable<ICreator<T>> creators)
    {
        _settingType = settingType;
        _creators = creators.ToDictionary(x => x.Type.ToLowerInvariant());
    }

    public T CreateProfile(ApplicationArguments arguments)
    {
        if (!arguments.TryGetValue(_settingType, out var section))
        {
            throw new ApplicationException($"No section {_settingType} was found.");
        }


        if (!_creators.TryGetValue(section.Type, out var creator))
            throw new ApplicationException($"No setting {typeof(T).Name} with the name of {section.Type}");

        return creator.Create(section);
    }
}
