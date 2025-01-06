using Zenex.DBContext;
using Microsoft.EntityFrameworkCore;
using Zenex.Authentication.IRespository;
using Zenex.Authentication.Respository;
using Zenex.Registration.IRespository;
using Zenex.Registration.Respository;
using Zenex.Master.IRespository;
using Zenex.Master.Respository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(p =>
    p.AddPolicy("corsapp", builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }));
builder.Services.AddDbContext<ZenexContext>(opts => opts.UseSqlServer(builder.Configuration["ConnectionString:AuthContextConnectionString"]));
builder.Services.AddScoped<IMasterRepository, MasterRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddTransient<IVendorOnBoardingRepository, VendorOnBoardingRepository>();
builder.Services.AddTransient<IIdentityRepository, IdentityRepository>();
builder.Services.AddTransient<IBankRepository, BankRepository>();
builder.Services.AddTransient<IContactRepository, ContactRepository>();
builder.Services.AddTransient<IActivityLogRepository, ActivityLogRepository>();
builder.Services.AddTransient<ITextRepository, TextRepository>();
builder.Services.AddTransient<IAttachmentRepository, AttachmentRepository>();
builder.Services.AddTransient<IServiceRepository, ServiceRepository>();


builder.Services.AddTransient<ITypeRepository, TypeRepository>();
builder.Services.AddTransient<IPostalRepository, PostalRepository>();
builder.Services.AddTransient<IRegisterIdentityRepository, RegisterIdentityRepository>();
builder.Services.AddTransient<IRegisterBankRepository, RegisterBankRepository>();
builder.Services.AddTransient<ITitleRepository, TitleRepository>();
builder.Services.AddTransient<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddTransient<IAppRepository, AppRepository>();
builder.Services.AddTransient<ILocationRepository, LocationRepository>();
builder.Services.AddTransient<IFieldMasterRepository, FieldMasterRepository>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("corsapp");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
