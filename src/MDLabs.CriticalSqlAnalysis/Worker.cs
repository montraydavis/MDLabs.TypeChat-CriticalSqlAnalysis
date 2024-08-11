using System.Diagnostics;
using MDLabs.CriticalSqlAnalysis.Handlers.Requests;
using MDLabs.CriticalSqlAnalysis.Models.Options;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MDLabs.CriticalSqlAnalysis;


/// <summary>
/// Worker service that processes project files.
/// </summary>
public class Worker : BackgroundService
{
    /// <summary>
    /// The mediator instance.
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    /// Gets the mediator instance.
    /// </summary>
    private IMediator mediator => _mediator;

    private readonly ILogger<Worker> _logger;
    private readonly WorkerOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="Worker"/> class.
    /// </summary>
    /// <param name="mediator">The mediator instance.</param>
    /// <param name="logger">The logger instance.</param>
    /// <param name="options">The worker options.</param>
    public Worker(IMediator mediator, ILogger<Worker> logger, IOptions<WorkerOptions> options)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Executes the background service.
    /// </summary>
    /// <param name="stoppingToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Debug.WriteLine("ExecuteAsync method started.");
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessSqlScriptsAsync(stoppingToken);

                // Wait for a certain period before the next run
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            Debug.WriteLine("Operation was canceled.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception occurred: {ex.Message}");
            _logger.LogError(ex, "An error occurred while executing the worker.");
        }
        finally
        {
            Debug.WriteLine("ExecuteAsync method completed.");
        }
    }

    /// <summary>
    /// Processes the SQL scripts asynchronously.
    /// </summary>
    /// <param name="stoppingToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task ProcessSqlScriptsAsync(CancellationToken stoppingToken)
    {
        Debug.WriteLine("ProcessSqlScriptsAsync method started.");
        try
        {
            Debug.WriteLine("Sending ReadProjectFilesRequest to mediator.");
            await _mediator.Send(new ReadProjectFilesRequest(_options.SqlScriptsPath), stoppingToken);
            Debug.WriteLine("ReadProjectFilesRequest sent successfully.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception occurred: {ex.Message}");
            _logger.LogError(ex, "An error occurred while processing SQL scripts.");
        }
        finally
        {
            Debug.WriteLine("ProcessSqlScriptsAsync method completed.");
        }
    }
}