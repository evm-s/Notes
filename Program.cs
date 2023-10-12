using Notes.Data;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        
        builder.Services.AddSqlite<NotesDbContext>("Data Source=Note.db");

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors(policy => policy.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}