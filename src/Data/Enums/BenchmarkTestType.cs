using HotChocolate;

namespace Planara.Benchmarks.Data.Enums;

/// <summary>
/// Тип тестирования
/// </summary>
[GraphQLDescription("Тип тестирования")]
public enum BenchmarkTestType
{
    /// <summary>
    /// Легкое тестирование
    /// </summary>
    [GraphQLDescription("Легкое тестирование")]
    Light,
    
    /// <summary>
    /// Среднее тестирование
    /// </summary>
    [GraphQLDescription("Среднее тестирование")]
    Medium,
    
    /// <summary>
    /// Тяжелое тестирование
    /// </summary>
    [GraphQLDescription("Тяжелое тестирование")]
    Heavy,
    
    /// <summary>
    /// Смешанное тестирование
    /// </summary>
    [GraphQLDescription("Смешанное тестирование")]
    Mixed
}