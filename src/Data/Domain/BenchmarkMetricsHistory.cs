namespace Planara.Benchmarks.Data.Domain;

/// <summary>
/// История метрик бенчмарк-теста для построения графиков.
/// </summary>
public class BenchmarkMetricsHistory
{
    /// <summary>
    /// Временные метки для построения графиков
    /// </summary>
    public List<double> TimeMs { get; set; } = [];

    /// <summary>
    /// Среднее количество FPS
    /// </summary>
    public List<double> AverageFps { get; set; } = [];

    /// <summary>
    /// Минимальное количество FPS
    /// </summary>
    public List<double> MinFps { get; set; } = [];

    /// <summary>
    /// Среднее время на отрисовку кадра
    /// </summary>
    public List<double> AverageFrameTime { get; set; } = [];

    /// <summary>
    /// Максиммальное время на отрисовку кадра
    /// </summary>
    public List<double> MaxFrameTime { get; set; } = [];

    /// <summary>
    /// Затраты памяти на отрисовку сцены в мегабайтах
    /// </summary>
    public List<double?> MemoryUsedMb { get; set; } = [];

    /// <summary>
    /// Количество вызовов draw calls
    /// </summary>
    public List<int> DrawCalls { get; set; } = [];

    /// <summary>
    /// Количество треугольников на сцене
    /// </summary>
    public List<int> Triangles { get; set; } = [];

    /// <summary>
    /// Количество объектов на сцене
    /// </summary>
    public List<int> ObjectsCount { get; set; } = [];
}