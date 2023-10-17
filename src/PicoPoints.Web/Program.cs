using PicoPoints;
using PicoPoints.Web;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddPicoPoints(c =>
{
    c.Endpoints = new List<Type>
    {
        typeof(TestEndpoint)
    };
});

var app = builder.Build();

app.UsePicoPoints();

app.Run();