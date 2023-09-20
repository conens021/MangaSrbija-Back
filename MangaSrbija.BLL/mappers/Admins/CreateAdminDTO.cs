using MangaSrbija.BLL.contracts.Enums;

namespace MangaSrbija.BLL.mappers.Admins
{
    public class CreateAdminDTO
    {
        public string Email { get; set; } = string.Empty;

        public string Passwrod { get; set; } = string.Empty;
        public List<AdminPermission> Permissions { get; set; }
    }
}
