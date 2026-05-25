using Planara.Benchmarks.Data.Enums;
using Planara.Common.Database.Domain;

namespace Planara.Benchmarks.Data.Domain;

/// <summary>
/// Результат отдельного бенчмарк-теста внутри запуска
/// </summary>
public class BenchmarkTestResult : BaseEntity
{
    /// <summary>
    /// ID запуска, к которому относится результат теста
    /// </summary>
    public Guid RunId { get; set; }

    /// <summary>
    /// Запуск, к которому относится результат теста
    /// </summary>
    public BenchmarkRun Run { get; set; } = null!;

    /// <summary>
    /// Тип тестирования
    /// </summary>
    public BenchmarkTestType Type { get; set; }

    /// <summary>
    /// Статус выполнения теста
    /// </summary>
    public BenchmarkTestStatus Status { get; set; } = BenchmarkTestStatus.Success;

    /// <summary>
    /// Сообщение об ошибке, если тест завершился неуспешно
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Длительность теста в миллисекундах
    /// </summary>
    public double DurationMs { get; set; }

    /// <summary>
    /// Количество отрисованных кадров
    /// </summary>
    public int Frames { get; set; }

    /// <summary>
    /// Среднее количество FPS
    /// </summary>
    public double AverageFps { get; set; }
    
    /// <summary>
    /// Минимальное количество FPS
    /// </summary>
    public double MinFps { get; set; }

    /// <summary>
    /// Среднее время на отрисовку кадра
    /// </summary>
    public double AverageFrameTime { get; set; }
    
    /// <summary>
    /// Максиммальное время на отрисовку кадра
    /// </summary>
    public double MaxFrameTime { get; set; }

    /// <summary>
    /// Количество объектов на сцене
    /// </summary>
    public int ObjectsCount { get; set; }

    /// <summary>
    /// Количество вызовов draw calls
    /// </summary>
    public int DrawCalls { get; set; }

    /// <summary>
    /// Количество треугольников на сцене
    /// </summary>
    public int Triangles { get; set; }

    /// <summary>
    /// Количество геометрий на сцене
    /// </summary>
    public int Geometries { get; set; }

    /// <summary>
    /// Количество текстур на сцене
    /// </summary>
    public int Textures { get; set; }

    /// <summary>
    /// Затраты памяти на отрисовку сцены в мегабайтах
    /// </summary>
    public double? MemoryUsedMb { get; set; }

    /// <summary>
    /// История метрик для построения графиков
    /// </summary>
    public BenchmarkMetricsHistory History { get; set; } = new();
}