using MyBookApi2.Services;

var builder = WebApplication.CreateBuilder(args);

// --- Service registration ---
builder.Services.AddControllers();
builder.Services.AddProblemDetails();     
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI: Scoped — one instance per HTTP request
builder.Services.AddScoped<IBookService, BookService>();

var app = builder.Build();

// --- Middleware pipeline ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStatusCodePages();   
app.MapControllers();

app.Run();
