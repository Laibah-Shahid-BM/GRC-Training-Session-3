using MyBookApi2.Services;

var builder = WebApplication.CreateBuilder(args);

// --- Service registration ---
builder.Services.AddControllers();
builder.Services.AddProblemDetails();     // wraps 4xx/5xx in RFC 9457 ProblemDetails JSON
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
app.UseStatusCodePages();   // ensures NotFound() → ProblemDetails JSON body
app.MapControllers();

app.Run();
