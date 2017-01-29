using CLog.Models.Timesheets;

namespace CLog.DataAccess.Mapping.Timesheets
{
    /// <summary>
    /// Represents the Date State model mapper.
    /// </summary>
    /// <seealso cref="CLog.DataAccess.Mapping.EntityMap{CLog.Models.Timesheets.DateState}" />
    internal class DateStateMap : EntityMap<DateState>
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

            Property(t => t.IsLocked)
                .HasColumnName("IsLocked")
                .HasColumnType("bit")
                .IsRequired();

            Property(t => t.IsPublicHoliday)
                .HasColumnName("IsPublicHoliday")
                .HasColumnType("bit")
                .IsRequired();
        }

        #endregion
    }
}
