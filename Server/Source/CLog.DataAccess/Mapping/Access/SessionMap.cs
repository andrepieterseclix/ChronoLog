using CLog.Models.Access;

namespace CLog.DataAccess.Mapping.Access
{
    /// <summary>
    /// Represents the Session model mapper.
    /// </summary>
    /// <seealso cref="CLog.DataAccess.Mapping.EntityMap{CLog.Models.Access.Session}" />
    internal class SessionMap : EntityMap<Session>
    {
        #region Methods

        /// <summary>
        /// Maps the columns.
        /// </summary>
        protected override void MapColumns()
        {
            Property(t => t.RefId)
                .HasColumnName("RefId")
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            Property(t => t.SessionKey)
                .HasColumnName("SessionKey")
                .HasColumnType("nvarchar")
                .HasMaxLength(512)
                .IsRequired();

            Property(t => t.UserId)
                .HasColumnName("UserId")
                .HasColumnType("bigint")
                .IsRequired();

            Property(t => t.LoginTimeUtc)
                .HasColumnName("LoginTimeUtc")
                .HasColumnType("datetime")
                .IsRequired();

            Property(t => t.LastActiveUtc)
                .HasColumnName("LastActiveUtc")
                .HasColumnType("datetime")
                .IsRequired();

            Property(t => t.IsActive)
                .HasColumnName("IsActive")
                .HasColumnType("bit")
                .IsRequired();
        }

        /// <summary>
        /// Maps the relationships.
        /// </summary>
        protected override void MapRelationships()
        {
            HasRequired(t => t.User)
                .WithMany(t => t.Sessions)
                .HasForeignKey(s => s.UserId)
                .WillCascadeOnDelete(true);
        }

        #endregion
    }
}
