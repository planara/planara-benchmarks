using HotChocolate;

namespace Planara.Benchmarks.Data.Enums;

/// <summary>
/// Статус тестирования
/// </summary>
[GraphQLDescription("Статус тестирования")]
public enum BenchmarkTestStatus
{
    /// <summary>
    /// Тесты прошли успешно
    /// </summary>
    [GraphQLDescription("Тесты прошли успешно")]
    Success,
    
    /// <summary>
    /// Тесты завершились с ошибкой
    /// </summary>
    [GraphQLDescription("Тесты завершились с ошибкой")]
    Failed,
    
    /// <summary>
    /// Тесты были пропущены
    /// </summary>
    [GraphQLDescription("Тесты были пропущены")]
    Skipped
}