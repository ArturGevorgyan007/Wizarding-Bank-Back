using DataAccess;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Services;
var  MyAllowSpecificOrigins = "*";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>  
{  
    options.AddPolicy(name: MyAllowSpecificOrigins,  
        policy  =>  
        {  
            policy.WithOrigins(MyAllowSpecificOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod(); // add the allowed origins  
        });  
});  

// Add services to the container.
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<CardServices>();
builder.Services.AddScoped<AccountServices>();
builder.Services.AddScoped<BusinessServices>();
builder.Services.AddScoped<LoanServices>();
builder.Services.AddScoped<TransactionServices>();
builder.Services.AddDbContext<WizardingBankDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("WizDB")));
builder.Services.AddCors(opt =>
        {
            opt.AddPolicy(name: "OpenAccess", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("OpenAccess");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(MyAllowSpecificOrigins);

app.Run();
