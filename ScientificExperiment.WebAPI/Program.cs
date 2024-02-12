using Microsoft.EntityFrameworkCore;
using WebApi_CSV.Mapper;
using WebApi_CSV.Middlewares;
using WebApi_CSV.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //Настройка контекста для взаимодействия с БД
        builder.Services.AddDbContext<DAL.DataContext>(options =>
        {
            options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnection"), sql => { });
            
        }, contextLifetime: ServiceLifetime.Scoped);
        builder.Services.AddScoped<FileService>();
        builder.Services.AddScoped<ModelBuilderService>();
        builder.Services.AddScoped<ProcessorValues>();
        builder.Services.AddScoped<DataServiceGet>();
        builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);
        var app = builder.Build();

        // при запуске приложения выполняются миграции
        using (var serviceScope = ((IApplicationBuilder)app).ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
        {
            if (serviceScope != null)
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DAL.DataContext>();
                context.Database.Migrate();
            }
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseGlobalErrorWrapper();

        app.MapControllers();

        app.Run();
    }
}