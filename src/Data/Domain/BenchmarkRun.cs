using Planara.Benchmarks.Data.Enums;
using Planara.Common.Database.Domain;

namespace Planara.Benchmarks.Data.Domain;

/// <summary>
/// Запуск бенчмарк-тестирования
/// </summary>
public class BenchmarkRun : BaseEntity
{
    /// <summary>
    /// ID пользователя, которому принадлежит запуск
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Название запуска тестов
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Время завершения запуска
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Общая длительность запуска в миллисекундах
    /// </summary>
    public double DurationMs { get; set; }

    /// <summary>
    /// Статус запуска тестирования
    /// </summary>
    public BenchmarkRunStatus Status { get; set; } = BenchmarkRunStatus.Completed;

    /// <summary>
    /// User-Agent браузера, в котором выполнялся тест
    /// </summary>
    public string? UserAgent { get; set; }

    /// <summary>
    /// Значение devicePixelRatio во время выполнения теста
    /// </summary>
    public double? DevicePixelRatio { get; set; }

    /// <summary>
    /// Результаты тестов, входящих в запуск
    /// </summary>
    public ICollection<BenchmarkTestResult> Tests { get; set; } = [];
}