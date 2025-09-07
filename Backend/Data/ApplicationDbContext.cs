using Backend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContext)
    : IdentityDbContext<AppUser>(dbContext);