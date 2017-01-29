using CLog.Models.Access;

namespace CLog.DataAccess.Mapping.Access
{
    /// <summary>
    /// Represents the User model mapper.
    /// </summary>
    /// <seealso cref="CLog.DataAccess.Mapping.EntityMap{CLog.Models.Access.User}" />
    internal class UserMap : EntityMap<User>
    {
        #region Methods

        /// <summary>
        /// Maps the columns.
        /// </summary>
        protected override void MapColumns()
        {
            Property(t => t.State)
                .HasColumnName("State")
                .HasColumnType("tinyint")
                .IsRequired();

            Property(t => t.UserName)
                .HasColumnName("UserName")
                .HasColumnType("nvarchar")
                .HasMaxLength(32)
                .IsRequired();

            Property(t => t.Password)
                .HasColumnName("Password")
                .HasColumnType("nvarchar")
                .HasMaxLength(512)
                .IsRequired();

            Property(t => t.Salt)
                .HasColumnName("Salt")
                .HasColumnType("nvarchar")
                .HasMaxLength(16)
                .IsRequired();

            Property(t => t.Name)
                .HasColumnName("Name")
                .HasColumnType("nvarchar")
                .HasMaxLength(128)
                .IsRequired();

            Property(t => t.Surname)
                .HasColumnName("Surname")
                .HasColumnType("nvarchar")
                .HasMaxLength(128)
                .IsRequired();

            Property(t => t.Email)
                .HasColumnName("Email")
                .HasColumnType("nvarchar")
                .HasMaxLength(128)
                .IsRequired();

            Property(t => t.ManagerId)
                .HasColumnName("ManagerId")
                .HasColumnType("bigint")
                .IsOptional();
        }

        #endregion
    }
}
