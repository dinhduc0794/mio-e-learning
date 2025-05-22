using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mio.ELearning.Core.Models;
using Mio.ELearning.Core.Repositories;

namespace Mio.ELearning.Data.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(LMSDbContext context, IConfiguration configuration, ILogger<UserRepository> logger) : base(context)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }
    
    private readonly LMSDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<UserRepository> _logger;
    private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();


    // Tìm người dùng theo email.
    public async Task<User> GetByEmailAsync(string email)
    {
        var user = await _context.Users
            .AsNoTracking() // Tránh cache thay đổi nếu có
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user != null)
        {
            Console.WriteLine($"[DEBUG] Email: {user.Email}, Password Hash: {user.Password}");
        }

        return user;
    }


    // Lấy danh sách người dùng theo RoleID (vai trò).
    public async Task<IEnumerable<User>> GetUsersByRoleAsync(int roleId)
    {
        return await _context.Users
            .Where(u => u.RoleId == roleId)
            .ToListAsync();
    }

    // Kiểm tra người dùng tồn tại hay không dựa trên email.
    public async Task<bool> CheckUserExistsAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email);
    }

    // Cập nhật trạng thái của người dùng.
    public async Task UpdateUserStatusAsync(int userId, bool status)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            user.IsActive = true;
            await _context.SaveChangesAsync();
        }
    }
    // Đăng ký người dùng mới
    public async Task<User> RegisterAsync(User user, string password)
    {
        // Kiểm tra email đã tồn tại chưa
        if (await CheckUserExistsAsync(user.Email))
            return null;

        // Lưu mật khẩu trực tiếp mã hóa
        user.Password = _passwordHasher.HashPassword(user, password);


        // Mặc định trạng thái là kích hoạt
        user.IsActive = true;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }


    // Đăng nhập người dùng (KIỂM TRA MẬT KHẨU TRỰC TIẾP)
    public async Task<User> LoginAsync(string email, string password)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            Console.WriteLine($"[ERROR] Đăng nhập thất bại: Email {email} không tồn tại.");
            return null;
        }

        //var hasher = new PasswordHasher<User>();
        //var result = hasher.VerifyHashedPassword(user, user.Password, password);
        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);


        if (result == PasswordVerificationResult.Failed)
        {
            Console.WriteLine($"[ERROR] Đăng nhập thất bại: Sai mật khẩu cho email {email}.");
            return null;
        }

        Console.WriteLine($"[DEBUG] Đăng nhập thành công: {user.Email} - RoleID: {user.RoleId}");

        // Trả lại thông tin cần thiết (nếu không muốn trả cả password)
        return new User
        {
            UserId = user.UserId,
            UserName = user.UserName,
            Email = user.Email,
            RoleId = user.RoleId,
            IsActive = user.IsActive,
            Password = user.Password
        };
    }


    public async Task UpdateUserRoleAsync(int userId, int newRoleId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            user.RoleId = newRoleId;
            await _context.SaveChangesAsync();
        }
    }
}
