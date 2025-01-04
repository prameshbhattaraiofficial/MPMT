using static Mpmt.Core.Dtos.PageModel.PageModel;

namespace Mpmt.Core.Dtos.PageModel
{
    /// <summary>
    /// The sort models.
    /// </summary>
    public class SortModels
    {
        //private string sortIconUp = "fa fa-long-arrow-up";
        //private string sortIcondown = "fa fa-long-arrow-down";
        private string sortIcondowncolor = "color:Gray";
        private string sortIconUpcolor = "Color:orange";
        private string sortIconUp = "fas fa-sort-alpha-up-alt";
        private string sortIcondown = "fas fa-sort-alpha-down";
        /// <summary>
        /// Gets or sets the sorted property.
        /// </summary>
        public string SortedProperty { get; set; }
        /// <summary>
        /// Gets or sets the sorted order.
        /// </summary>
        public sortOrder SortedOrder { get; set; }
        private List<SortableColumns> sortableColumns = new List<SortableColumns>();
        /// <summary>
        /// Adds the column.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <param name="IsdefaultColumn">If true, isdefault column.</param>
        public void AddColumn(string columns, bool IsdefaultColumn = false)
        {
            SortableColumns temp = this.sortableColumns.Where(x => x.ColumnName.ToLower() == columns.ToLower()).SingleOrDefault();
            if (temp == null)
            {
                sortableColumns.Add(new SortableColumns { ColumnName = columns });
            }
            if (IsdefaultColumn == true || sortableColumns.Count == 1)
            {
                SortedProperty = columns;
                SortedOrder = sortOrder.Ascending;
            }
        }
        /// <summary>
        /// Gets the column.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <returns>A SortableColumns.</returns>
        public SortableColumns GetColumn(string columns)
        {
            SortableColumns temp = this.sortableColumns.Where(x => x.ColumnName.ToLower() == columns.ToLower()).SingleOrDefault();
            if (temp == null)
            {
                sortableColumns.Add(new SortableColumns { ColumnName = columns });
            }
            return temp;
        }

        /// <summary>
        /// Applysorts the.
        /// </summary>
        /// <param name="sortexpression">The sortexpression.</param>
        public void Applysorts(string sortexpression)
        {
            if (sortexpression == "")
            {
                sortexpression = this.SortedProperty;
            }
            sortexpression = sortexpression.ToLower();
            foreach (SortableColumns sortablecolumn in this.sortableColumns)
            {
                sortablecolumn.SortIcon = "";
                sortablecolumn.SortExpression = sortablecolumn.ColumnName;
                if (sortexpression == sortablecolumn.ColumnName.ToLower())
                {
                    this.SortedOrder = sortOrder.Ascending;
                    this.SortedProperty = sortablecolumn.ColumnName;
                    sortablecolumn.SortIcon = this.sortIcondown;
                    sortablecolumn.SortIconColor = this.sortIcondowncolor;
                    sortablecolumn.SortExpression = sortablecolumn.ColumnName + "_desc";
                }
                if (sortexpression == sortablecolumn.ColumnName.ToLower() + "_desc")
                {
                    this.SortedOrder = sortOrder.Descending;
                    this.SortedProperty = sortablecolumn.ColumnName;
                    sortablecolumn.SortIcon = this.sortIconUp;
                    sortablecolumn.SortIconColor = this.sortIconUpcolor;
                    sortablecolumn.SortExpression = sortablecolumn.ColumnName;
                }
            }

        }

        //internal void Applysorts(object sortExpression)
        //{
        //    throw new NotImplementedException();
        //}
    }

    /// <summary>
    /// The sortable columns.
    /// </summary>
    public class SortableColumns
    {
        /// <summary>
        /// Gets or sets the column name.
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// Gets or sets the sort expression.
        /// </summary>
        public string SortExpression { get; set; }
        /// <summary>
        /// Gets or sets the sort icon.
        /// </summary>
        public string SortIcon { get; set; }
        /// <summary>
        /// Gets or sets the sort icon color.
        /// </summary>
        public string SortIconColor { get; set; }
    }
}
