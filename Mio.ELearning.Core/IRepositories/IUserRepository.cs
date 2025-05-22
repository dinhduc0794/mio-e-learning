using Mio.ELearning.Core.Models;

namespace Mio.ELearning.Core.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    // Tìm người dùng bằng email.
    Task<User> GetByEmailAsync(string email);
    // Lấy danh sách người dùng theo vai trò (RoleID)
    Task<IEnumerable<User>> GetUsersByRoleAsync(int roleId);

    // Kiểm tra người dùng đã tồn tại chưa.
    Task<bool> CheckUserExistsAsync(string email);
    
    // Cập nhật trạng thái người dùng
    Task UpdateUserStatusAsync(int userId, bool status);

    // Đăng ký người dùng mới
    Task<User> RegisterAsync(User user, string password);

    // Đăng nhập người dùng
    Task<User> LoginAsync(string email, string password);
    // Thêm các phương thức sau cho OTP
    Task UpdateUserRoleAsync(int userId, int newRoleId);
}