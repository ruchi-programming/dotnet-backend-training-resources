using ProductApi.Repositories;

WebApplicationBuilder builder =
    WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<
    IProductRepository,
    InMemoryProductRepository>();

WebApplication app = builder.Build();

app.MapControllers();

app.Run();
