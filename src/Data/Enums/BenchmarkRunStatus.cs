using HotChocolate;

namespace Planara.Benchmarks.Data.Enums;

/// <summary>
/// Статус запуска тестирования
/// </summary>
[GraphQLDescription("Статус запуска тестирования")]
public enum BenchmarkRunStatus
{
    /// <summary>
    /// Запуск прошел успешно
    /// </summary>
    [GraphQLDescription("Запуск прошел успешно")]
    Completed,
    
    /// <summary>
    /// Запуск завершился с ошибкой
    /// </summary>
    [GraphQLDescription("Запуск завершился с ошибкой")]
    Failed,
}