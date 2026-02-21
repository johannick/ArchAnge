using Abstraction.Database;
using Abstraction.Repository;
using ArchAnge.ServiceDefaults.Middleware;
using ArchAnge.ServiceDefaults.Repository;
using ArchAnge.ServiceDefaults.Version;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ServiceDiscovery;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using System.Data;
using Abstraction;
using Npgsql;
using Model.User;
using ArchAnge.ServiceDefaults.Sql;
using Model.Contact;
using Model.Interest;
using Model.Profile;
using Model.Hub;
using Asp.Versioning;

using ApiVersioningOptions = Asp.Versioning.ApiVersioningOptions;

namespace ServiceDefaults;

public static class Extensions
{
    /// <summary>
    /// AddServiceDefaults
    /// <list type="bullet">
    ///    <item> Telemetry </item>
    ///    <item> Api Versionning </item>
    ///    <item> Health Checks </item>
    ///    <item> Service discovery </item> 
    /// </list>
    /// </summary>
    /// <typeparam name="TBuilder"> <see cref="IHostApplicationBuilder"/> </typeparam>
    /// <param name="builder"> Application Builder </param>
    /// <returns> <paramref name="builder"/></returns>
    public static TBuilder AddServiceDefaults<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.ConfigureOpenTelemetry();

        builder.ConfigureVersionning();

        builder.AddDefaultHealthChecks();

        builder.Services.AddServiceDiscovery();

        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            http.AddStandardResilienceHandler();
            http.AddServiceDiscovery();
        });

        builder.Services.Configure<ServiceDiscoveryOptions>(options =>
        {
            options.AllowedSchemes = ["https"];
        });

        builder.Services.AddTransient<MiddlewareFactory>();
        builder.Services.AddTransient<LoggerMiddleware>();
        builder.Services.AddTransient<RoleAuthorizationMiddleware>();

        builder.Services.AddSignalRCore();

        return builder;
    }

    /// <summary>
    /// Configure Api Versionning (for concurrent running version)
    /// </summary>
    /// <typeparam name="TBuilder"> <see cref="IHostApplicationBuilder"/></typeparam>
    /// <param name="builder"> Application  builder </param>
    /// <returns> <paramref name="builder"/> </returns>
    public static TBuilder ConfigureVersionning<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        var versionFormat = "VVV";
        var constraintName = "releaseVersion";

        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = ReleaseProvider.Default.ApiVersion;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        })
        .AddMvc()
        .AddApiExplorer(options =>
        {
            options.DefaultApiVersion = ReleaseProvider.Default.ApiVersion;
            options.GroupNameFormat = versionFormat;
            options.SubstituteApiVersionInUrl = true;
            options.RouteConstraintName = constraintName;
            options.ApiVersionParameterSource = new UrlSegmentApiVersionReader();
        });

        builder.Services.AddRouting(options => options.ConstraintMap.Add(constraintName, typeof(ReleaseVersionConstraint)));

        return builder;
    }

    /// <summary>
    /// Configure telemetry (for Aspire Dashboard)
    /// </summary>
    /// <typeparam name="TBuilder"><see cref="IHostApplicationBuilder"/></typeparam>
    /// <param name="builder"> Application builder </param>
    /// <returns> <paramref name="builder"/></returns>
    public static TBuilder ConfigureOpenTelemetry<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        var telemetryBuilder = builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation();
            })
            .WithTracing(tracing =>
            {
                tracing.AddSource(builder.Environment.ApplicationName)
                    .AddAspNetCoreInstrumentation()
                    // Uncomment the following line to enable gRPC instrumentation (requires the OpenTelemetry.Instrumentation.GrpcNetClient package)
                    //.AddGrpcClientInstrumentation()
                    .AddHttpClientInstrumentation();
            });

        return builder.AddOpenTelemetryExporters(telemetryBuilder);
    }

    /// <summary>
    /// The exporter registered will be added as the last processor in the pipeline for logging and tracing
    /// To add Azure monitoring add this lines :
    /// <code>
    /// // requires package Azure.Monitor.OpenTelemetry.AspNetCore
    /// if (!string.IsNullOrWhiteSpace(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
    /// {
    ///     telemetryBuilder.UseAzureMonitor();
    /// } 
    /// </code>
    /// </summary>
    /// <typeparam name="TBuilder">  <see cref="IHostApplicationBuilder"/> </typeparam>
    /// <param name="builder"> Application builder </param>
    /// <param name="telemetryBuilder">Open Telemetry builder </param>
    /// <returns> <paramref name="builder"/> </returns>
    private static TBuilder AddOpenTelemetryExporters<TBuilder>(this TBuilder builder, IOpenTelemetryBuilder telemetryBuilder) where TBuilder : IHostApplicationBuilder
    {
        if (!string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]))
        {
            telemetryBuilder.UseOtlpExporter();
        }

        return builder;
    }

    /// <summary>
    /// Self check
    /// </summary>
    /// <typeparam name="TBuilder"> <see cref="IHostApplicationBuilder"/></typeparam>
    /// <param name="builder"> Application builder </param>
    /// <returns> <paramref name="builder"/></returns>
    public static TBuilder AddDefaultHealthChecks<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddHealthChecks().AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

        return builder;
    }

    /// <summary>
    /// - Map health checks
    /// - Add common middlewares
    /// </summary>
    /// <param name="app"> </param>
    /// <returns> <see cref="WebApplication"/> </returns>
    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        app.UseMiddleware<LoggerMiddleware>();
        app.UseMiddleware<RoleAuthorizationMiddleware>();

        if (!app.Environment.IsDevelopment())
        {
            return app;
        }
        app.MapHealthChecks("/health");
        app.MapHealthChecks("/alive", new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("live")
        });
        return app;
    }

    /// <summary>
    /// Map swaggerUI and swagger.json for all released versions
    /// </summary>
    /// <param name="app"></param>
    /// <exception cref="FileNotFoundException"></exception>
    public static WebApplication MapSwagger(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            return app;
        }
        var cssUri = "/swagger.css";
        var serviceAssembly = typeof(Extensions).Assembly;
        var name = serviceAssembly.GetName().Name + ".Swagger.swagger.css";
        using var stream = serviceAssembly.GetManifestResourceStream(name);
        using var reader = new StreamReader(stream ?? throw new FileNotFoundException());
        var cssContent = reader.ReadToEnd();

        app.Map(cssUri, builder => builder.Run(async context => await context.Response.WriteAsync(cssContent)));
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            ReleaseProvider.ApiVersionDescriptions.ToList().ForEach(description =>
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"Version {description.GroupName}");
            });
            options.InjectStylesheet(cssUri);
            options.RoutePrefix = string.Empty;
        });
        return app;
    }

    /// <summary>
    /// Delete where according to entity Id
    /// </summary>
    /// <typeparam name="TEntity"> Entity type </typeparam>
    /// <param name="repository"> repository </param>
    /// <param name="entity"> entity key holder </param>
    /// <returns> async list of paired deleted entities, see <see cref="IBulkDeleteRepository{TEntity}"/> </returns>
    public static async Task<List<Tuple<bool, TEntity>>> DeleteWhere<TEntity>(this RepositoryConnectionContext<TEntity> repository, IEntity entity)
        where TEntity : class
    {
        var items = await repository.Where(entity, nameof(IEntity<Guid>)).ToListAsync(repository.Context.RequestAborted);

        return await repository.Delete(items).ToListAsync(repository.Context.RequestAborted);
    }

    /// <summary>
    /// Update where according to entity Id
    /// </summary>
    /// <typeparam name="TEntity"> Entity type </typeparam>
    /// <param name="repository"> repository </param>
    /// <param name="entity"> entity key holder </param>
    /// <returns> async list of paired updated entities, see <see cref="IBulkUpdateRepository{TEntity}"/> </returns>
    public static async Task<List<Tuple<bool, TEntity>>> UpdateWhere<TEntity>(this RepositoryConnectionContext<TEntity> repository, IEntity entity)
        where TEntity : class
    {
        var items = await repository.Where(entity, nameof(IEntity<Guid>)).ToListAsync(repository.Context.RequestAborted);

        return await repository.Update(items).ToListAsync(repository.Context.RequestAborted);
    }

    /// <summary>
    /// Add 
    /// <list type="bullet">
    ///     <item> Automapper profiles </item>
    ///     <item> Connection options </item>
    ///     <item> Repositories </item>
    /// </list>
    /// </summary>
    /// <typeparam name="TBuilder"></typeparam>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static TBuilder AddMapping<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        var options = new SqlQueryBuilderOption
        {
            OpenQuote = "\"",
            CloseQuote = "\"",
            JoinSeparator = ".",
            SetSeparator = ",",
            ParameterPrefix = "@",
            TablePrefix = "public"
        };

        var connectionFunc = new Func<NpgsqlConnectionStringBuilder?, IDbConnection>(builder => new NpgsqlConnection(builder?.ConnectionString));
        var npgBuilderFunc = new Func<ConnectionStringOption?, NpgsqlConnectionStringBuilder>(options => new NpgsqlConnectionStringBuilder
        {
            Host = options?.Host,
            Port = options?.Port ?? 5432,
            Database = options?.Database,
            Username = options?.Username,
            Password = options?.Password,
            CancellationTimeout = (int)(options?.CancellationTimeout.TotalMilliseconds ?? double.NegativeZero),
            Timeout = (int)(options?.Timeout.TotalSeconds ?? double.NegativeZero),
            Pooling = true,
            Multiplexing = true
        });

        builder.Services.AddSingleton(options);
        builder.Services.AddSingleton(provider => npgBuilderFunc(provider.GetService<ConnectionStringOption>()));
        builder.Services.AddSingleton<IPasswordHasher<UserModel>, PasswordHasher<UserModel>>();
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        builder.Services.AddScoped(provider => connectionFunc(provider.GetService<NpgsqlConnectionStringBuilder>()));
        builder.Services.AddScoped(provider => new ConnectionRequestContext { DatabaseConnection = provider.GetService<IDbConnection>() });

        return builder.AddRepositoryDefaults(options);
    }

    /// <summary>
    /// Add repository when TEntity is not IEntity<TKey>
    /// </summary>
    /// <typeparam name="TEntity"> Database Entity </typeparam>
    /// <paramref name="builder"/>
    /// <paramref name="options"/>
    private static void AddRepository<TEntity>
        (this IHostApplicationBuilder builder
        , SqlQueryBuilderOption options)
        where TEntity : class
    {
        var queryBuilder = new SqlQueryBuilder<TEntity>(options);
        var singles = queryBuilder.Provider.Keys.Select(queryBuilder.Provider.Alias);
        var uniques = queryBuilder.Provider.Unique.Select(queryBuilder.Provider.Alias);

        builder.Services.AddSingleton<IDatabaseQueryBuilder<TEntity>>(queryBuilder);
        builder.Services.AddScoped<IRepository<TEntity>>(provider => new RepositoryConnectionContext<TEntity>(provider.GetRequiredService<ConnectionRequestContext>(), queryBuilder, singles, uniques));
    }

    private static TBuilder AddRepositoryDefaults<TBuilder>(this TBuilder builder, SqlQueryBuilderOption options) where TBuilder : IHostApplicationBuilder
    {
        builder.AddRepository<CountryModel>(options);
        builder.AddRepository<InterestCategoryModel>(options);
        builder.AddRepository<ProfilePictureModel>(options);
        builder.AddRepository<ProfileInterestModel>(options);
        builder.AddRepository<ProfileAddressModel>(options);
        builder.AddRepository<ProfilePhoneModel>(options);
        builder.AddRepository<MatchModel>(options);
        builder.AddRepository<RoomParticipantModel>(options);
        builder.AddRepository<LocationModel>(options);
        builder.AddRepository<UserModel>(options);
        builder.AddRepository<ProfileModel>(options);
        builder.AddRepository<InterestModel>(options);
        builder.AddRepository<MessageModel>(options);
        builder.AddRepository<PhonePatternModel>(options);
        builder.AddRepository<PhoneNumberModel>(options);
        builder.AddRepository<RoomModel>(options);

        return builder;
    }
}
