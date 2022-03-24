using backend_aoz.Repos;
using backend_aoz.Workers;


var fetchEGLDPriceThread = AssetPriceTagWorker.GetThread();
fetchEGLDPriceThread.Start();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IReferralCodeRepo, FileReferralCodeRepo>();
builder.Services.AddSingleton<IAssetPriceTagRepo, FileAssetPriceTagRepo>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "corsPolicy",
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:3000",
                                              "https://www.ageOfZalmoxis.com");
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseCors("corsPolicy");
app.Run();
