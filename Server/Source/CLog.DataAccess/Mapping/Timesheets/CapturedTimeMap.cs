using CLog.Models.Timesheets;

namespace CLog.DataAccess.Mapping.Timesheets
{
    /// <summary>
    /// Represents the Captured Time model mapper.
    /// </summary>
    /// <seealso cref="CLog.DataAccess.Mapping.EntityMap{CLog.Models.Timesheets.CapturedTime}" />
    internal class CapturedTimeMap : EntityMap<CapturedTime>
    {
        #region Methods

        /// <summary>
        /// Maps the columns.
        /// </summary>
        protected override void MapColumns()
        {
            Property(t => t.Date)
                .HasColumnName("Date")
                .HasColumnType("datetime")
                .IsRequired();

            Property(t => t.HoursWorked)
                .HasColumnName("HoursWorked")
                .HasColumnType("tinyint")
                .IsRequired();

            Ignore(t => t.IsLocked);

            Ignore(t => t.IsPublicHoliday);

            Ignore(t => t.IsEnabled);

            Property(t => t.UserId)
                .HasColumnName("UserId")
                .HasColumnType("bigint")
                .IsRequired();
        }

        /// <summary>
        /// Maps the relationships.
        /// </summary>
        protected override void MapRelationships()
        {
            HasRequired(t => t.User)
                .WithMany(t => t.CapturedTimeItems)
                .HasForeignKey(s => s.UserId)
                .WillCascadeOnDelete(true);
        }

        #endregion
    }
}
