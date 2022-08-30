using System.Data;

namespace Plan.Plandokument
{
    public static class DataTableHelpers
    {

        /// <summary>
        /// Sorterar datatabell efter önskad kolumn och sorteringsordning. Tar endast hänsyn till en kolumn och en sorteringsordning.
        /// </summary>
        /// <param name="sortableTable">
        /// Datatabell att sortera.
        /// </param>
        /// <param name="sortableColumn">
        /// Kolumn i datatabell som datatabellen ska sorteras på.
        /// </param>
        /// <param name="sortOrder">
        /// Sorteringsordning DESC = fallande | ASC = stigande
        /// </param>
        /// <returns>
        /// Sorterad datatabell.
        /// </returns>
        internal static DataTable getTableSorted(DataTable sortableTable, string sortableColumn, string sortOrder)
        {
            DataView v = sortableTable.DefaultView;
            v.Sort = sortableColumn + " " + sortOrder;
            sortableTable = v.ToTable();
            return sortableTable;
        }

        /// <summary>
        /// Sorterar datatabell efter önskad kolumn och sorteringsordning. Tar hänsyn till två kolumner och sorteringsordning.
        /// </summary>
        /// <param name="sortableTable">
        /// Datatabell att sortera.
        /// </param>
        /// <param name="sortableColumn1">
        /// Kolumn i datatabell som datatabellen ska sorteras på.
        /// </param>
        /// <param name="sortOrder1">
        /// Sorteringsordning DESC = fallande | ASC = stigande
        /// </param>
        /// <param name="sortableColumn2">
        /// Kolumn i datatabell som datatabellen ska sorteras på.
        /// </param>
        /// <param name="sortOrder2">
        /// Sorteringsordning DESC = fallande | ASC = stigande
        /// </param>
        /// <returns>
        /// Sorterad datatabell.
        /// </returns>
        internal static DataTable getTableSorted(DataTable sortableTable, string sortableColumn1, string sortOrder1, string sortableColumn2, string sortOrder2)
        {
            DataView v = sortableTable.DefaultView;
            v.Sort = sortableColumn1 + " " + sortOrder1 + ", " + sortableColumn2 + " " + sortOrder2;
            sortableTable = v.ToTable();
            return sortableTable;
        }
    }
}